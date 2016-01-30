using UnityEngine;
using System.Collections;
using System;

public class TimedTask : Interactable
{
    public float Time;

    protected bool running;

    public override void Interact(Player player)
    {
        ClearAtt();
        if (running) return;
        running = true;
        StartCoroutine(Do(player));
    }

    protected virtual IEnumerator Do(Player player)
    {
        yield return new WaitForSeconds(Time);
        Done = true;
        running = false;
    }
}
