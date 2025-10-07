using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MyGame.Networking.Prediction;
using UnityEngine.Animations;
using System;

public class AgentController : NetworkBehaviour, IInputReceivable
{

    // Controllers
    private IMoveable moveController;
    private ILookable lookController;
    private IWeapon weapon;
    private PlayerInputController inputController;
    Camera localCam; // owner’s camera for look angle calc

    private bool isFiring = false;

    private void Awake()
    {
        moveController = GetComponent<IMoveable>();
        lookController = GetComponent<ILookable>();
        weapon = GetComponentInChildren<IWeapon>();

        // null checks
        if (moveController == null) Debug.LogError($"[{name}] missing IMoveable");
        if (lookController == null) Debug.LogError($"[{name}] missing ILookable");
        if (weapon == null) Debug.LogWarning($"[{name}] no IWeapon found in children");
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        enabled = IsOwner;

        if (IsOwner)
        {
            inputController = GetComponent<PlayerInputController>();
            if (inputController == null)
                Debug.LogError($"[{name}] missing PlayerInputController");
            else
                inputController.Initialize(this);

            // NEW: set the local camera used for mouse->world raycasts
            localCam = Camera.main;
        }
    }

    private void FixedUpdate()
    {
       // Only the owner should drive local firing prediction
        if (IsOwner && isFiring)
            weapon?.Fire();
    }

    public void SetMoveInput(Vector2 input)
    {
        if (!IsOwner) return;
        moveController?.Move(input);
    }

    public void SetLookInput(Vector2 input, bool isWorldPosition)
    {
        if (!IsOwner) return;
        lookController?.SetLookDirection(input, isWorldPosition);

    }

    public void SetFire(bool isPressed)
    {
        if (!IsOwner) return;
        isFiring = isPressed;
    }

    public void SetAction(bool isPressed)
    {
        // Hook to action logic
    }

    public void SetToggle(bool isPressed)
    {
        // Hook to movement mode or stance
    }

    public void SetTarget(bool isPressed)
    {
        // Hook to targeting system
    }
}