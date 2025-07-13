using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AgentController : NetworkBehaviour, IInputReceivable
{
    private IMoveable moveController;
    private ILookable lookController;
    private IWeapon weapon;
    private PlayerInputController inputController;

    private bool isFiring = false;

    private void Awake()
    {
        moveController = GetComponent<IMoveable>();
        lookController = GetComponent<ILookable>();
        weapon = GetComponentInChildren<IWeapon>();

        if (moveController == null)
            Debug.LogError($"[{name}] missing IMoveable");
        if (lookController == null)
            Debug.LogError($"[{name}] missing ILookable");
        if (weapon == null)
            Debug.LogWarning($"[{name}] no IWeapon found in children");
    }

    public override void OnNetworkSpawn()
    {
        // Only the owning client should hook up inputs
        if (!IsOwner) return;
        inputController = GetComponent<PlayerInputController>();

        if (inputController == null)
            Debug.LogError($"[{name}] missing PlayerInputController");

        inputController.Initialize(this);
    }

    // IInputReceivable → client drives its own movement
    public void SetMoveInput(Vector2 input)
    {
        if (!IsOwner) return;
        moveController?.Move(input);
    }

    // IInputReceivable → client drives its own look
    public void SetLookInput(Vector2 input, bool isWorldPosition)
    {
        if (!IsOwner) return;
        lookController?.SetLookDirection(input, isWorldPosition);
    }
    
    public void SetFire(bool isPressed)
    {
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

    private void FixedUpdate()
    {
        if (isFiring)
        {
            weapon?.Fire();
        }
    }

    /*  // COMMENTING SERVER RPC IMPLEMENTATION FOR NOW WHILE PROTOTYPING
        // IInputReceivable → movement now goes through the server
        public void SetMoveInput(Vector2 input)
        {
            if (!IsOwner) return;

            if (IsServer)
            {
                // Host/server runs movement locally
                moveController?.Move(input);
            }
            else
            {
                // Clients send their input to the server
                SendMoveInputServerRpc(input);
            }
        }

        [ServerRpc]
        private void SendMoveInputServerRpc(Vector2 input, ServerRpcParams rpcParams = default)
        {
            // Server applies the input to its Rigidbody2D
            moveController?.Move(input);
        }

        // Look: owner sends to server, server applies
        public void SetLookInput(Vector2 input, bool isWorldPosition)
        {
            if (!IsOwner) return;

            // 1) Rotate instantly on this client:
            lookController?.SetLookDirection(input, isWorldPosition);

            // 2) Then fire the ServerRpc so the server also applies it:
            if (IsServer)
            {
                // host
                lookController?.SetLookDirection(input, isWorldPosition);
            }
            else
            {
                SendLookInputServerRpc(input, isWorldPosition);
            }
        }

        [ServerRpc]
        private void SendLookInputServerRpc(Vector2 input, bool isWorldPosition, ServerRpcParams rpcParams = default)
        {
            lookController?.SetLookDirection(input, isWorldPosition);
        }
     */

}