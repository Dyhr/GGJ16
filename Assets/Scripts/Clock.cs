using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Clock : MonoBehaviour
{
    public bool Running = true;
    public int StartTimeHour = 5;
    public int StartTimeMinute = 0;
    public int EndTimeHour = 6;
    public int EndTimeMinute = 0;
    public float MinuteTime = 1;

    private int hour;
    private int minute;
    private float timer;

    public Text text;

    private void Start()
    {
        hour = StartTimeHour;
        minute = StartTimeMinute;
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        while (true)
        {
            yield return new WaitForSeconds(MinuteTime);
            if (Running) { 
                minute++;
                while (minute >= 60)
                {
                    minute -= 60;
                    hour++;
                }

                text.text = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (minute < 10 ? "0" + minute : minute.ToString());

                if (hour >= EndTimeHour && minute >= EndTimeMinute)
                {
                    Running = false;

                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Control = false;
                    Debug.Log("You Lose");
                }
            }
        }
    }
}
