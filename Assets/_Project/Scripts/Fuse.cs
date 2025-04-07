using System;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    internal void RbDisalbe()
    {
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _collider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioHelper.PlaySound("FuseImpact", collision.contacts[0].point);
    }
}
