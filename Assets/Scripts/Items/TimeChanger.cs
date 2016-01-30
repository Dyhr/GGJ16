using UnityEngine;
using System.Collections;

public class TimeChanger : TimedTask
{
    public string AndThen;
    public float TimeToCoffee = 2;

    protected override IEnumerator Do(Player player)
    {
        yield return new WaitForSeconds(Time);
        ClearAtt();
        Done = true;
        yield return new WaitForSeconds(TimeToCoffee);
        Done = false;
        Name = AndThen;
        Attention(player.CanDo(Name), player.AttPrefab);
        running = false;
    }
}
