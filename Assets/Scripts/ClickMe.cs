using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ClickMe : MonoBehaviour
{
    public Color Color;
    public Image Ring;
    public Image Mark;

    public float Size1;
    public float Size2;

    public float Speed = 1;
    public int Steps = 60;
    public AnimationCurve Curve;

    private void Start()
    {
        StartCoroutine(Loop());
        Ring.color = Color;
        Mark.color = Color;
    }

    private IEnumerator Loop()
    {
        while (true) { 
            float time = 0;
            while (time < 1)
            {
                yield return new WaitForSeconds(1 / (float)Steps);
                time += Time.deltaTime * Speed;
                var size = Mathf.LerpUnclamped(Size1, Size2, Curve.Evaluate(time));
                Ring.rectTransform.sizeDelta = new Vector2(size,size);
            }
            time = 0;
            while (time < 1)
            {
                yield return new WaitForSeconds(1 / (float)Steps);
                time += Time.deltaTime*Speed;
                var size = Mathf.LerpUnclamped(Size2, Size1, Curve.Evaluate(time));
                Ring.rectTransform.sizeDelta = new Vector2(size, size);
            }
        }
    }
}
