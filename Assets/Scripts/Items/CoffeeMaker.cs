using UnityEngine;
using System.Collections;
using System;

public class CoffeeMaker : Interactable
{
    public float TimeToCoffee = 2;

    public override void Interact(Player player)
    {
        ClearAtt();
        Done = true;
        StartCoroutine(WaitForCoffee(player));
    }

    private IEnumerator WaitForCoffee(Player player)
    {
        yield return new WaitForSeconds(TimeToCoffee);
        Done = false;
        Name = "Drink coffee";
        Attention(player.CanDo(Name), player.AttPrefab);
    }
}
