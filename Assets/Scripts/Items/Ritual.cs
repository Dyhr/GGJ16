using UnityEngine;
using System.Collections;
using System;

public class Ritual : TimedTask
{
    public string AndThen;
    public string AndThen2;
    public float TimeToCoffee = 2;

    protected override IEnumerator Do(Player player)
    {
        yield return new WaitForSeconds(Time);
        ClearAtt();
        Done = true;
        yield return new WaitForSeconds(TimeToCoffee);
        Debug.Log(Name+" "+AndThen+" "+AndThen2);
        if (Name != AndThen && Name != AndThen2)
        {
            Debug.Log("1");
            Done = false;
            Name = AndThen;
            Attention(player.CanDo(Name), player.AttPrefab);
        }
        else if (Name != AndThen2)
        {
            Debug.Log("1");
            Done = false;
            Name = AndThen2;
            Attention(player.CanDo(Name), player.AttPrefab);
        }
        running = false;
    }
}
