using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class Cam : MonoBehaviour
{
    public Player Player;
    public Vector3 Origin;
    public Vector3 Offset = new Vector3(-10, 20, -10);
    public AnimationCurve Curve;
    public float DefaultSize;
    public float ZoomSize;

    private float time;
    private float prepos;
    private float pretarget;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        var pos = !Player.doing ? Origin : Player.transform.position;

        var target = pos + Offset;

        transform.position = Vector3.Lerp(transform.position, target, 0.2f);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, !Player.doing ? DefaultSize : ZoomSize, 0.2f);
        transform.LookAt(pos);
    }
}
