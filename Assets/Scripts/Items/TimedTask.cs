using UnityEngine;
using System.Collections;
using System;

public class TimedTask : Interactable
{
    public float Time;

    public override void Interact(Player player)
    {
        ClearAtt();
        StartCoroutine(Do(player));
    }

    protected virtual IEnumerator Do(Player player)
    {
        yield return new WaitForSeconds(Time);
        Done = true;
    }
}
