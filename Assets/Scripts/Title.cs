using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public static int Day;

    public bool Skip;

    private void Start()
    {
        if (Skip)
        {
            gameObject.SetActive(false);
            return;
        }
        player.enabled = Clock.Running = false;
        if (Day == 0)
        {
            Background.color = BackgroundLight;
            Text.color = TextLight;
        }
        else
        {
            Background.color = BackgroundDark;
            Text.color = TextDark;
            Text.text = "";
        }
    }

    public void Click()
    {
        Submit.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);

        StartCoroutine(SwitchTo());
    }

    internal void NextDay()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        Day++;
        Text.text = "";
        StartCoroutine(FadeLight());
        yield return new WaitForSeconds(4);
        var index = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(index + " of  " + SceneManager.sceneCountInBuildSettings);
        if (index+1 >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("You win!");
            StartCoroutine(FadeAway());
            yield return new WaitForSeconds(1.5f);
            Text.text = "you made it, "+Name.text;
            StartCoroutine(FadeDark());
        }
        else
        {
            Debug.Log("Next scene!");
            Text.text = "tomorrow commences..";
            StartCoroutine(FadeDark());
            yield return new WaitForSeconds(3);
            StartCoroutine(FadeAway());
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(++index);
        }
    }

    private IEnumerator SwitchTo()
    {
        Color old = Text.color;
        int steps = 30;
        float time = 0;
        for (var i = 0; i < steps; ++i)
        {
            yield return new WaitForSeconds(1 / (float)steps);
            time += 1 / (float)steps;
            Text.color = Color.Lerp(old, BackgroundLight, time);
        }
        Text.color = BackgroundLight;
        switch (Day)
        {
            case 0: Text.text = "monday"; break;
            case 1: Text.text = "tuesday"; break;
            case 2: Text.text = "wednesday"; break;
            case 3: Text.text = "thursday"; break;
            case 4: Text.text = "friday"; break;
            default: Text.text = "someday"; break;
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

        Debug.Log("Ready to go!");
        player.enabled = Clock.Running = true;
    }
    private IEnumerator FadeLight()
    {
        Color oldB = Background.color;
        Color newB = BackgroundLight;

        int steps = 30;
        float time = 0;
        for (var i = 0; i < steps; ++i)
        {
            yield return new WaitForSeconds(1 / (float)steps);
            time += 1 / (float)steps;
            Background.color = Color.Lerp(oldB, newB, time);
        }
        Background.color = newB;
    }
    private IEnumerator FadeDark()
    {
        Color oldT = Text.color;
        Color oldB = Background.color;
        Color newT = TextDark;
        Color newB = BackgroundDark;

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
    }
    private IEnumerator FadeAway()
    {
        Color oldT = Text.color;
        Color oldB = Background.color;
        Color newT = BackgroundDark;
        Color newB = BackgroundDark;

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
    }
}
