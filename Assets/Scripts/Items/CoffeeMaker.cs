using UnityEngine;
using System.Collections;
using System;

public class CoffeeMaker : Interactable
{
    public float TimeToCoffee = 2;

    public override void Interact(Player player)
    {
        Done = true;
        StartCoroutine(WaitForCoffee());
    }

    private IEnumerator WaitForCoffee()
    {
        yield return new WaitForSeconds(TimeToCoffee);
        Done = false;
        Name = "Drink coffee";
    }
}
