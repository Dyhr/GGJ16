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
    public static int Stress;
    public static string SName;

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
            Background.color = BackgroundLight;
            Text.color = TextLight;
            Text.text = "the next day.";
            Name.text = SName;
            Name.enabled = false;
            Clock.MinuteTime -= 0.2f * Day + 0.1f * Stress;
        }
    }

    public void Click()
    {
        Submit.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
        SName = Name.text;

        StartCoroutine(SwitchTo());
    }

    public void Credits(Text text)
    {
        text.text = "Art:\nAmanda James\nCode:\nRasmus Dyhr Larsen\n\nMusic:\nhttp://newgrounds.com/audio/listen/635055\n\nSounds:\nfreesounds.org";
    }

    internal void NextDay(int stress)
    {
        Stress += stress;
        StartCoroutine(FadeAndLoad());
    }

    internal void Lose()
    {
        StartCoroutine(FadeAndDie());
    }

    private IEnumerator FadeAndLoad()
    {
        Text.text = "";
        StartCoroutine(FadeLight());
        yield return new WaitForSeconds(4);
        var index = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(index + " of  " + SceneManager.sceneCountInBuildSettings);
        if (Day == 4)
        {
            Day = 0;
            Debug.Log("You win!");
            StartCoroutine(FadeAway());
            yield return new WaitForSeconds(1.5f);
            Text.text = "you made it, "+ Name.text.ToLower() + ".\ngood job.";
            StartCoroutine(FadeDark());
            yield return new WaitForSeconds(3);
            StartCoroutine(FadeAway());
            yield return new WaitForSeconds(3);
            Text.text = "";
            StartCoroutine(FadeLight());
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(0);
        }
        else
        {
            Day++;
            Debug.Log("Next scene!");
            Text.text = "you are "+StressName()+".\nyou're doing good.";
            StartCoroutine(FadeDark());
            yield return new WaitForSeconds(3);
            StartCoroutine(FadeAway());
            yield return new WaitForSeconds(3);
            Text.text = "";
            StartCoroutine(FadeLight());
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(0);
        }
    }
    private string StressName()
    {
        switch (Stress)
        {
            case 0:
                return "not at all stressed";
            case 1:
                return "a tiny bit stressed";
            case 2:
                return "slightly stressed";
            case 3:
                return "a bit stressed";
            case 4:
                return "kinda stressed";
            case 5:
                return "stressed";
            case 6:
                return "pretty stressed";
            case 7:
                return "very stressed";
            case 8:
                return "too stressed";
            case 9:
                return "extremely stressed";
            default:
                return "way too stressed";
        }
    }
    private IEnumerator FadeAndDie()
    {
        Day = 0;
        Text.text = "";
        StartCoroutine(FadeAway());
        yield return new WaitForSeconds(2);

        Text.text = "you late, " + Name.text.ToLower() + ".\nyou're fired.";
        StartCoroutine(FadeDark());
        yield return new WaitForSeconds(5);
        StartCoroutine(FadeAway());
        yield return new WaitForSeconds(1);
        Text.text = "";
        StartCoroutine(FadeLight());
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
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
        Text.text += " morning.\nmake coffee.\ntake a shower.\nleave for work.";
        yield return new WaitForSeconds(0.2f);
        time = 0;
        for (var i = 0; i < steps; ++i)
        {
            yield return new WaitForSeconds(1 / (float)steps);
            time += 1 / (float)steps;
            Text.color = Color.Lerp(BackgroundLight, TextLight, time);
        }
        Text.color = TextLight;
        yield return new WaitForSeconds(4f);
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
