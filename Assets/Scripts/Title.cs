using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    public Player player;
    public Clock Clock;
    public Image Background;
    public Text Text;
    public InputField Name;
    public Button Submit;

    public Color BackgroundLight;
    public Color BackgroundDark;
    public Color TextLight;
    public Color TextDark;
    public AnimationCurve Curve;

    public int Day;

    public bool Skip;

    private void Start()
    {
        if (Skip)
        {
            Destroy(gameObject);
            return;
        }
        player.enabled = Clock.Running = false;
        Background.color = BackgroundLight;
        Text.color = TextLight;
    }

    public void Click()
    {
        Submit.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);

        StartCoroutine(SwitchTo("Monday"));
    }

    private IEnumerator SwitchTo(string day)
    {
        Color old = Text.color;
        int steps = 30;
        float time = 0;
        for (var i = 0; i < steps; ++i)
        {
            yield return new WaitForSeconds(1 / (float)steps);
            time += 1 / (float)steps;
            Text.color = Color.Lerp(old,BackgroundLight,time);
        }
        Text.color = BackgroundLight;
        switch (Day)
        {
            case 0: Text.text = "Monday"; break;
            case 1: Text.text = "Tuesday"; break;
            default: Text.text = "Someday"; break;
        }
        yield return new WaitForSeconds(0.2f);
        time = 0;
        for (var i = 0; i < steps; ++i)
        {
            yield return new WaitForSeconds(1 / (float)steps);
            time += 1 / (float)steps;
            Text.color = Color.Lerp(BackgroundLight, TextLight, time);
        }
        Text.color = TextLight;
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color oldT = Text.color;
        Color oldB = Background.color;
        Color newT = oldT;
        Color newB = oldB;
        newT.a = 0;
        newB.a = 0;

        int steps = 30;
        float time = 0;
        for (var i = 0; i < steps; ++i)
        {
            yield return new WaitForSeconds(1 / (float)steps);
            time += 1 / (float)steps;
            Text.color = Color.Lerp(oldT, newT, time);
            Background.color = Color.Lerp(oldB, newB, time);
        }
        Text.color = newT;
        Background.color = newB;

        player.enabled = Clock.Running = true;
    }
}
