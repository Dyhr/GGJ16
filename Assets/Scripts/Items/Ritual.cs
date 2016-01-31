using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Ritual : TimedTask
{
    public string AndThen;
    public string AndThen2;
    public float TimeToCoffee = 2;
    public SpriteRenderer Circle;
    public Transform Mug;
    public AnimationCurve MugCurve;
    public float MugSpeed;
    public float MugRotate;

    public Transform Candle;
    public float CandleSpeed;

    public AudioChorusFilter Chorus;
    public AudioDistortionFilter Distort;

    public override void Interact(Player player)
    {
        ClearAtt();
        if (running) return;
        running = true;
        if (Anchor.magnitude > 0)
        {
            oldPos = player.transform.position;
            player.transform.position = Anchor;
            player.GetComponent<Rigidbody>().isKinematic = true;
        }
        StartCoroutine(Do(player));
    }

    protected override IEnumerator Do(Player player)
    {
        if (Name == AndThen)
        {
            Transform[] candles = new Transform[5];
            for(int i = 0; i < 5; ++i)
            {
                float angle = i * (Mathf.PI * 2) / 5;
                var p = transform.position + (Mathf.Cos(angle) * Vector3.forward + Mathf.Sin(angle) * Vector3.right) * Radius;
                candles[i] = (Transform)Instantiate(Candle, p, Quaternion.identity);
                yield return new WaitForSeconds(CandleSpeed);
            }
            for (int i = 0; i < 5; ++i)
            {
                candles[i].GetComponentInChildren<ParticleSystem>().Play();
                yield return new WaitForSeconds(CandleSpeed);
            }
        }
        else if (Name == AndThen2)
        {
            var a = transform.position - Vector3.up*0.5f;
            var b = transform.position + Vector3.up;
            var mug = (Transform)Instantiate(Mug, a, Quaternion.identity);
            var time = 0.0f;
            StartCoroutine(MugMe(mug));
            while (time < 1)
            {
                yield return new WaitForEndOfFrame();
                time += UnityEngine.Time.deltaTime * MugSpeed;
                mug.position = Vector3.LerpUnclamped(a, b, MugCurve.Evaluate(time));
            }
            mug.position = b;
        }
        else
        {
            yield return new WaitForSeconds(Time);
        }

        ClearAtt();
        Done = true;
        Circle.enabled = true;
        yield return new WaitForSeconds(TimeToCoffee);
        if (Name != AndThen && Name != AndThen2)
        {
            Done = false;
            Name = AndThen;
            Attention(player.CanDo(Name), player.AttPrefab);
        }
        else if (Name != AndThen2)
        {
            Done = false;
            Name = AndThen2;
            Attention(player.CanDo(Name), player.AttPrefab);
        }

        running = false;
        if (Anchor.magnitude > 0)
        {
            player.transform.position = oldPos;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private IEnumerator MugMe(Transform mug)
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            mug.Rotate(0, UnityEngine.Time.deltaTime * MugRotate, 0);
        }
    }
}
