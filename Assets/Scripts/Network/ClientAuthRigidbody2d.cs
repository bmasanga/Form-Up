using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D))]
public class ClientAuthRigidbody2D : NetworkBehaviour
{
    private Rigidbody2D rb;

    private NetworkVariable<Vector2> netVelocity =
        // note: singular “ReadPermission” and singular “WritePermission”
        new NetworkVariable<Vector2>(
            default,                                 // initial value
            NetworkVariableReadPermission.Everyone,  // who may read
            NetworkVariableWritePermission.Owner     // who may write
        );

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            // owner drives its own velocity
            netVelocity.Value = rb.velocity;
        }
        else
        {
            // everyone else applies that velocity
            rb.velocity = netVelocity.Value;
        }
    }
}