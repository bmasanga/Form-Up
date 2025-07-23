using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MyGame.Networking.Prediction;
using UnityEngine.Animations;

public class AgentController : NetworkBehaviour, IInputReceivable
{
    //[SerializeField] private float reconciliationThreshold = 1f; // units in world-space
    [SerializeField] float smoothingSpeed = 5f;

    // Debug tools
    [SerializeField] private bool showServerDebug = false;
    [SerializeField] private GameObject debugMarker;

    private IMoveable moveController;
    private ILookable lookController;
    private IWeapon weapon;
    private PlayerInputController inputController;

    private bool isFiring = false;

    // Client prediction fields
    private const float ServerTickRate = 60f;      // ticks per second
    private NetworkTimer networkTimer;  // drives our fixed‐rate
    private const int BufferSize = 1024;
    private CircularBuffer<InputPayload> clientInputBuffer;
    private StatePayload targetState;    // from server

    // Raw input buffering
    private Vector2 pendingMoveInput;
    private Vector2 pendingLookInput;
    private bool pendingIsWorld;
    private bool pendingFire;

    //need to read back velocity on server
    private Rigidbody2D rb;

    private void Awake()
    {
        // Grab your Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        moveController = GetComponent<IMoveable>();
        lookController = GetComponent<ILookable>();
        weapon = GetComponentInChildren<IWeapon>();

        // null checks
        if (rb == null)
            Debug.LogError($"[{name}] missing Rigidbody2D for state sync");
        if (moveController == null)
            Debug.LogError($"[{name}] missing IMoveable");
        if (lookController == null)
            Debug.LogError($"[{name}] missing ILookable");
        if (weapon == null)
            Debug.LogWarning($"[{name}] no IWeapon found in children");

        // Initializing network buffers
        networkTimer = new NetworkTimer(ServerTickRate);
        clientInputBuffer = new CircularBuffer<InputPayload>(BufferSize);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // Only owner should run prediction loop
        enabled = IsOwner;

        if (IsOwner)
        {
            inputController = GetComponent<PlayerInputController>();
            if (inputController == null)
                Debug.LogError($"[{name}] missing PlayerInputController");
            else
                inputController.Initialize(this);   // ← hook up your IInputReceivable callbacks
        }
        // Orphans the debug marker from parent
        debugMarker.transform.SetParent(null);
    }


    // IInputReceivable callbacks now just store the values…
    public void SetMoveInput(Vector2 input)
    {
        if (!IsOwner) return;
        //moveController?.Move(input);
        pendingMoveInput = input; // <- buffer for next tick
    }

    public void SetLookInput(Vector2 input, bool isWorldPosition)
    {
        if (!IsOwner) return;
        //lookController?.SetLookDirection(input, isWorldPosition);
        pendingLookInput = input; // <- buffer for next tick
        pendingIsWorld = isWorldPosition;
    }

    public void SetFire(bool isPressed)
    {
        //isFiring = isPressed;
        pendingFire = isPressed; // <- buffer for next tick
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

        if (!IsOwner) return;

        // advance your NetworkTimer with physics-time
        networkTimer.Update(Time.fixedDeltaTime);

        // run exactly one ClientTick per physics step
        while (networkTimer.ShouldTick())
            ClientTick();
    }

    private void ClientTick()
    {
        int tick = networkTimer.CurrentTick;
        int bufIndex = tick % BufferSize;

        // Predict locally using your existing controllers
        moveController?.Move(pendingMoveInput);
        lookController?.SetLookDirection(pendingLookInput, pendingIsWorld);

         // **capture the final facing angle**
        float lookAngle = transform.rotation.eulerAngles.z;       

        // Package the tick’s inputs
        var payload = new InputPayload
        {
            Tick = tick,
            MoveInput = pendingMoveInput,
            LookInput = pendingLookInput,
            IsWorldLook = pendingIsWorld,
            LookAngle = lookAngle,
            Fire = pendingFire
        };
        clientInputBuffer.Add(payload, bufIndex);
       

        if (payload.Fire)
            weapon?.Fire();

        Debug.Log($"Tick {payload.Tick}: input = {payload.MoveInput}");

        // 3) Send the inputs to the server
        SendInputServerRpc(payload);
    }
    
    [ServerRpc]
    private void SendInputServerRpc(InputPayload payload, ServerRpcParams rpcParams = default)
    {
        // Run on server

        //moveController?.Move(payload.MoveInput);
        //lookController.SetLookDirection(payload.LookInput, payload.IsWorldLook);
        /*
        if (!payload.IsWorldLook)
        {
            // stick: compute angle & snap rotation
            float angle = Mathf.Atan2(payload.LookInput.y, payload.LookInput.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // mouse: still schedule world-point rotation via your LookController
            lookController.SetLookDirection(payload.LookInput, true);
        }
        */

        // 1) Rotate authoritatively on the server exactly as the client does:
        //lookController.SnapLook(payload.LookInput, payload.IsWorldLook);

        // 1) Immediately apply the exact look‐angle the client computed:
        transform.rotation = Quaternion.Euler(0f, 0f, payload.LookAngle);

        // 2) Then move in that freshly‐rotated direction
        moveController.Move(payload.MoveInput);

        if (payload.Fire)
            weapon?.Fire();

        // Package server “ground truth”
        var statePayload = new StatePayload
        {
            Tick = payload.Tick,
            Position = transform.position,
            Rotation = transform.rotation,
            Velocity = rb.velocity,
            AngularVel = rb.angularVelocity
        };

        // Send it back to the owner
        BroadcastStateClientRpc(statePayload);
    }

    [ClientRpc]
    private void BroadcastStateClientRpc(StatePayload statePayload, ClientRpcParams rpcParams = default)
    {
        if (!IsOwner) return;

        Debug.Log($"[Client] Received server state tick {statePayload.Tick}");
        targetState = statePayload;

        if (showServerDebug && debugMarker != null)
        {
            debugMarker.transform.position = statePayload.Position;
        }
    }
    
    private void LateUpdate()
    {
        // only pure clients smooth toward the server
        if (IsServer || !IsClient || targetState.Tick == 0)
            return;

        // Blend toward the server’s last authoritative position each frame:
        transform.position = Vector3.Lerp(
            transform.position,
            targetState.Position,
            Time.deltaTime * smoothingSpeed    //raise to snap quicker
        );
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetState.Rotation,
            Time.deltaTime * smoothingSpeed
        );
    }
}