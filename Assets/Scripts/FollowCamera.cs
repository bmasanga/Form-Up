using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FollowCamera : MonoBehaviour
{
    public Transform Target { get; set; }

    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothTime = 0.1f;
    private Vector3 _velocity;

    void FixedUpdate()
    {
        if (Target == null) return;

        // Desired camera world position:
        Vector3 desired = Target.position + offset;
        transform.position = desired;
        
/* COMMENTING THIS CLIENT LOGIC FOR NOW
        // Determine if this is a pure client (not host)
        bool isRemoteClient = NetworkManager.Singleton != null
                              && NetworkManager.Singleton.IsClient
                              && !NetworkManager.Singleton.IsServer;

        if (isRemoteClient)
        {
            // Remote clients: smooth-damp between frames
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desired,
                ref _velocity,
                smoothTime
            );
        }
        else
        {
            // Host (or dedicated server): snap exactly to the target after physics
            transform.position = desired;
        } */
    }
}
