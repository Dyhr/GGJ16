using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Human : MonoBehaviour
{
    public float Height;
    public float Speed;

    [HideInInspector]
    public Vector3 InputControl;
    [HideInInspector]
    public bool InputAim;

    [HideInInspector]
    public Vector3 Forward = Vector3.forward;
    [HideInInspector]
    public Vector3 Right = Vector3.right;

    [HideInInspector]
    public Action IdleLook;
    [HideInInspector]
    public bool LockRot;

    private Rigidbody _rigidbody;
    private float aimTime;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var move = (Forward*InputControl.z + Right*InputControl.x).normalized;
        if (InputControl.magnitude == 0)
        {
            if (IdleLook != null) IdleLook();
        }
        else if (!LockRot)
        {
            if(move.magnitude > 0)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(move), 0.4f);
        }
        _rigidbody.velocity = move*Speed;

        InputControl = Vector3.zero;
        InputAim = false;
        LockRot = false;
    }
}
