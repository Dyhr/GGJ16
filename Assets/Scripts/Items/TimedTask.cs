using UnityEngine;
using System.Collections;
using System;

public class TimedTask : Interactable
{
    public float Time;

    public override void Interact(Player player)
    {
        StartCoroutine(Do());
    }

    private IEnumerator Do()
    {
        yield return new WaitForSeconds(Time);
        Done = true;
    }
}
