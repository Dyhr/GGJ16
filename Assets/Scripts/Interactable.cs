using UnityEngine;
using System.Collections;

public class Interactable : Aimable
{
    public float InteractionDistance;
    public string Name;
    public bool Done;

    private ClickMe attention;

    public override void Click(Player player)
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= InteractionDistance)
            Interact(player);
    }
    public virtual void Interact(Player player)
    {
        ClearAtt();
        Done = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, InteractionDistance);
    }

    public void Attention(string name, ClickMe prefab)
    {
        ClearAtt();
        if (Name != name) return;

        attention = (ClickMe)Instantiate(prefab, transform.position, Camera.main.transform.rotation);
    }
    public void ClearAtt()
    {
        if(attention != null)
        {
            Destroy(attention.gameObject);
            attention = null;
        }
    }
}
