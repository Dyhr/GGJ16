using UnityEngine;
using System.Collections;

public class Bed : Interactable
{
    public Vector3 Spawn;

    public override void Interact(Player player)
    {
        Done = true;
        player.transform.rotation = Quaternion.identity;
        player.transform.position = Spawn;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.path = null;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Spawn, 0.1f);
    }
}
