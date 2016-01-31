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

    protected override IEnumerator Do(Player player)
    {
        yield return new WaitForSeconds(Time);
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
        if (Anchor.magnitude > 0) { 
            player.transform.position = oldPos;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
