using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class PlayerCameraSetup : NetworkBehaviour
{
    private GameObject playerCameraInstance;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Load the camera prefab from Resources
            GameObject cameraPrefab = Resources.Load<GameObject>("PlayerCamera");

            if (cameraPrefab != null)
            {
                playerCameraInstance = Instantiate(cameraPrefab);

                // Find the virtual camera in the prefab
                CinemachineVirtualCamera vcam = playerCameraInstance.GetComponentInChildren<CinemachineVirtualCamera>();
                if (vcam != null)
                {
                    vcam.Follow = transform;
                    vcam.LookAt = transform; // optional, depending on how you want to aim
                }
            }
            else
            {
                Debug.LogError("PlayerCameraRig prefab not found in Resources folder.");
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