using UnityEngine;
using System.Collections;

public class Aimable : MonoBehaviour
{
    public float Radius;

    private Transform spherechild;
    private SphereCollider sphere;

    public virtual void Click(Player player)
    {
        
    }

    private void Start()
    {
        spherechild = (new GameObject("Aim Collider")).transform;
        spherechild.parent = transform;
        spherechild.gameObject.layer = LayerMask.NameToLayer("Target");
        spherechild.localPosition = Vector3.zero;
        sphere = spherechild.gameObject.AddComponent<SphereCollider>();
        sphere.radius = Radius;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
