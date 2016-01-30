using UnityEngine;
using System.Collections;

public class Interactable : Aimable
{
    public float InteractionDistance;
    public string Name;
    public bool Done;

    public override void Click(Player player)
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= InteractionDistance)
            Interact(player);
    }
    public virtual void Interact(Player player)
    {
        Done = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, InteractionDistance);
    }
}
