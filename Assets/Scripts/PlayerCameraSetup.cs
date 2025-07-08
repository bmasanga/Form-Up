using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class PlayerCameraSetup : NetworkBehaviour
{
    [SerializeField] private GameObject cameraPrefab;

    private GameObject playerCameraInstance;

    public override void OnNetworkSpawn()
    {
        if (IsOwner && cameraPrefab != null)
        {
            playerCameraInstance = Instantiate(cameraPrefab);

            var vcam = playerCameraInstance.GetComponentInChildren<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Follow = transform;
                // vcam.LookAt = transform;
            }
        }
    }

    private void OnDestroy()
    {
        if (IsOwner && playerCameraInstance != null)
        {
            Destroy(playerCameraInstance);
        }
    }
}