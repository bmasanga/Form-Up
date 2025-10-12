using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MyGame.Networking.Prediction;
using System;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;


public class AgentController : NetworkBehaviour, IInputReceivable
{

    // Controllers
    private IMoveable moveController;
    private ILookable lookController;
    private IWeapon weapon;
    private PlayerInputController inputController;
    private Camera localCam; // owner’s camera for look angle calc
    private Rigidbody2D rb;
    private ClientNetworkTransform clientNetworkTransform;

    // Netcode general
    private NetworkTimer networkTimer;
    const float serverTickRate = 60f; // 60FPS
    const int bufferSize = 1024;
    private int _lastEnqueuedTick = -1;     // Anti-duplication / basic validation on the server

    // Raw input buffering (owner fills these through IInputReceivable)
    private Vector2 pendingMoveInput;
    private Vector2 pendingLookInput;
    private bool    pendingFire;

    // Netcode client specific
    CircularBuffer<StatePayload> clientStateBuffer; // tracks the state we're at at every tick
    CircularBuffer<InputPayload> clientInputBuffer; // tracks the input used at every tick
    StatePayload lastServerState; // tracks the last state of the server processed
    StatePayload lastProcessedState; // tracks the last successful state reconciled on the client side
    //StatePayload _targetRemoteState;  // last state for non-owners

    // Netcode server specific
    CircularBuffer<StatePayload> serverStateBuffer; // This is how the server keeps track of all its simulated states
    Queue<InputPayload> serverInputQueue; // This is the queue to process all inputs as they come in

    [Header("Netcode")]
    [SerializeField] float reconciliationThreshold = 10f;
    [SerializeField] GameObject serverMarker;
    //[SerializeField] GameObject clientMarker;

    void Awake()
    {
        // Initialize controllers
        moveController = GetComponent<IMoveable>();
        lookController = GetComponent<ILookable>();
        weapon = GetComponentInChildren<IWeapon>();
        rb = GetComponent<Rigidbody2D>();
        clientNetworkTransform = GetComponent<ClientNetworkTransform>();


        // null checks
        if (moveController == null) Debug.LogError($"[{name}] missing IMoveable");
        if (lookController == null) Debug.LogError($"[{name}] missing ILookable");
        if (weapon == null) Debug.LogWarning($"[{name}] no IWeapon found in children");

        // Initilize Netcode variables
        networkTimer = new NetworkTimer(serverTickRate);
        clientStateBuffer = new CircularBuffer<StatePayload>(bufferSize);
        clientInputBuffer = new CircularBuffer<InputPayload>(bufferSize);

        serverStateBuffer = new CircularBuffer<StatePayload>(bufferSize);
        serverInputQueue = new Queue<InputPayload>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            inputController = GetComponent<PlayerInputController>();
            if (inputController == null)
                Debug.LogError($"[{name}] missing PlayerInputController");
            else
                inputController.Initialize(this);

            localCam = Camera.main;

            //Set the local camera used for mouse->world raycasts
            if (lookController is LookController lc && localCam != null)
            {
                lc.SetCamera(localCam);
            }
        }

        // Only the server (authoritative) and the local owner (for prediction) should simulate
        if (rb != null)
            rb.simulated = IsServer || IsOwner;
    }

    void FixedUpdate()
    {
        // Always advance timer in physics time (both client and server use it)
        networkTimer.Update(Time.fixedDeltaTime);

        while (networkTimer.ShouldTick())
        {
            if (IsClient && IsOwner)
            {
                HandleClientTick();
            }

            if (IsServer)
            {
                HandleServerTick();
            }
        }
    }

    /* void LateUpdate()
    {
        if (IsOwner) return;
        if (_targetRemoteState.tick == 0) return;

        // Make sure non-owners aren’t running local physics
        if (rb != null) rb.simulated = false;

        transform.position = Vector3.Lerp(
            transform.position,
            _targetRemoteState.position,
            Time.deltaTime * 8f
        );
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            _targetRemoteState.rotation,
            Time.deltaTime * 8f
        );
    } */
 
    void HandleServerTick()
    {
        if (!IsServer) return;

        // As long as there's input in the queue from the client, process it
        var bufferIndex = -1;
        InputPayload inputPayload = default;

        // While loop will run until queue is empty
        while (serverInputQueue.Count > 0)
        {
            // On every iteration, deque an input payload
            inputPayload = serverInputQueue.Dequeue();

            // Determine buffer index based on the tick of the payload
            bufferIndex = inputPayload.tick % bufferSize;

            StatePayload statePayload;

            if (IsHost)
            {
                // Host already predicted this locally in HandleClientTick().
                // Do NOT simulate again—just snapshot current authoritative state.
                statePayload = MakeState(inputPayload.tick);
            }
            else
            {
                // Dedicated server or remote owner: simulate on server
                statePayload = ProcessMovement(inputPayload, applyRotation: true);
            }

            serverStateBuffer.Add(statePayload, bufferIndex);
        }

        // If bufferIndex moved at all, send whatever has been processed back to the client
        if (bufferIndex == -1) return;
        SendToClientRpc(serverStateBuffer.Get(bufferIndex));
    }

    [ClientRpc]
    void SendToClientRpc(StatePayload statePayload)
    {
        // Server marker for debugging
        if (serverMarker != null)
            serverMarker.transform.position = statePayload.position;

        // Ensure the message targets THIS object instance
        if (statePayload.networkObjectId != NetworkObjectId)
            return;

        if (!IsOwner) return;
        // Set the last server state equal to what ever was provided
        lastServerState = statePayload;

        // Non-owner smoothing target
        //_targetRemoteState = statePayload;
    }

    void HandleClientTick()
    {
        if (!IsClient || !IsOwner) return;

        // Get current tick from the timer
        var currentTick = networkTimer.CurrentTick;

        // Based on tick, figure out where the buffer index is in Input Buffer to store input data
        var bufferIndex = currentTick % bufferSize;

        // 1) Predict visually on the client (smooth look). Here we just feed it the current input so it can update this frame.
        if (lookController != null)
        {
            lookController.SetLookDirection(pendingLookInput);
        }

        // 2) Capture the exact facing angle reached this frame
        float lookAngleDeg = transform.eulerAngles.z;
        LookController lc = lookController as LookController;
        if (lc != null)
        {
            lookAngleDeg = lc.GetLookAngleDeg();
        }

        // 3) Predict local movement & firing ONCE for this tick
        if (moveController != null)
        {
            moveController.Move(pendingMoveInput);
        }
        if (pendingFire && weapon != null)
        {
            weapon.Fire();
        }

         // 4) Build + send the input payload for this tick
        InputPayload inputPayload = new InputPayload()
        {
            tick            = currentTick,
            timestamp       = DateTime.UtcNow,   // optional, but nice for diagnostics
            networkObjectId = NetworkObjectId,
            moveInput       = pendingMoveInput,
            lookAngle       = lookAngleDeg,
            fire            = pendingFire,
        };

        // Send input payload to buffer
        clientInputBuffer.Add(inputPayload, bufferIndex);

        // Send payload to server so it knows what the client has been doing 
        SendToServerRpc(inputPayload);

        // 5) Record predicted state WITHOUT a second simulation
        StatePayload predicted = MakeState(currentTick);

        // Take the state payload and save it client side for reconciliation
        clientStateBuffer.Add(predicted, bufferIndex);

        HandleServerReconciliation();
    }

    bool ShouldReconcile()  // There are two reasons we should reconcile
    {
        // 1) is this a brand new server state other than the default aka never initialized?
        bool isNewServerState = !lastServerState.Equals(default);

        // 2) is this state different than the last processed state?
        bool isLastStateUndefinedOrDifferent = lastProcessedState.Equals(default)
                                                || !lastProcessedState.Equals(lastServerState);

        return isNewServerState && isLastStateUndefinedOrDifferent;
    }

    void HandleServerReconciliation()
    {
        // Decide if we should reconcile, if not bail out
        if (!ShouldReconcile()) return;

        float positionError;
        int bufferIndex;

        bufferIndex = lastServerState.tick % bufferSize;

        // If buffer is close to zero, then there's not enough information to reconcile with, bail out
        if (bufferIndex - 1 < 0) return; 
        
        // If we are the host of the game, set the state to rewind back to as the most recent state in the buffer
        StatePayload rewindState = IsHost ? serverStateBuffer.Get(bufferIndex - 1) : lastServerState;
        StatePayload clientState = IsHost ? clientStateBuffer.Get(bufferIndex - 1) : clientStateBuffer.Get(bufferIndex);

        // Find the distance between the two states
        positionError = Vector3.Distance(rewindState.position, clientStateBuffer.Get(bufferIndex).position);

        // If distance is within a certain threshold, 
        if (positionError > reconciliationThreshold)
        {
            ReconcileState(rewindState);
        }

        lastProcessedState = rewindState;
    }

    void ReconcileState(StatePayload rewindState)
    {
        // Reset position to the state that we're rewinding to
        transform.position  = rewindState.position;
        transform.rotation  = rewindState.rotation;
        rb.velocity         = rewindState.velocity;
        rb.angularVelocity  = rewindState.angularVelocity;

        // If rewind state is the last state received from server, then stop here, usually means we are the host
        if (!rewindState.Equals(lastServerState)) return;
        
        // Put rewind state into buffer as if we had performed that state in the beginning
        clientStateBuffer.Add(rewindState, rewindState.tick % bufferSize);
        
        // Replay all inputs from the rewind state to the current state
        int tickToReplay = lastServerState.tick;
        
        while (tickToReplay < networkTimer.CurrentTick)
        {
            int bufferIndex = tickToReplay % bufferSize;
            StatePayload statePayload = ProcessMovement(clientInputBuffer.Get(bufferIndex), applyRotation: true);
            clientStateBuffer.Add(statePayload, bufferIndex);
            tickToReplay++;
        }
    }

    [ServerRpc] // This runs on the server (or host) when the owner calls it.
    private void SendToServerRpc(InputPayload input, ServerRpcParams rpcParams = default)
    {
        // Client marker for debugging
        //clientMarker.transform.position = input.position;

        // Sanity: this input must belong to *this* NetworkObject
        if (input.networkObjectId != NetworkObjectId)
        {
            // Wrong target (ignore silently or log)
            return;
        }

        // Optional: drop out-of-order/duplicate ticks (simple guard)
        if (input.tick <= _lastEnqueuedTick)
        {
            return;
        }
        _lastEnqueuedTick = input.tick;

        // Enqueue for HandleServerTick() to process
        if (serverInputQueue != null)
        {
            serverInputQueue.Enqueue(input);
        }
    }

    StatePayload ProcessMovement(InputPayload input, bool applyRotation) // This processes the movement based on the inputs, only run by server and by client during reconciliation
    {
        // 1. Rotate only when authoritative/replay
        if (applyRotation)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, input.lookAngle);
        }

        // 2. Apply movement input through your MoveController
        if (moveController != null)
        {
            moveController.Move(input.moveInput);
        }

        // 3. Handle any action inputs (fire, dash, etc.)
        if (input.fire)
            weapon?.Fire();

        return new StatePayload()
        {
            tick            = input.tick,
            networkObjectId = NetworkObjectId,
            position        = transform.position,
            rotation        = transform.rotation,
            velocity        = rb.velocity,
            angularVelocity = rb.angularVelocity
        };
    }

    private StatePayload MakeState(int tick) // Helper method for the client to take a snapshot of the state after making the prediction from the inputs
    {
        return new StatePayload()
        {
            tick            = tick,
            networkObjectId = NetworkObjectId,
            position        = transform.position,
            rotation        = transform.rotation,
            velocity        = rb.velocity,
            angularVelocity = rb.angularVelocity
        };
    }


    public void SetMoveInput(Vector2 input)
    {
        if (!IsOwner) return;
        pendingMoveInput = input;
    }

    public void SetLookInput(Vector2 input)
    {
        if (!IsOwner) return;
        pendingLookInput = input;
    }

    public void SetFire(bool isPressed)
    {
        if (!IsOwner) return;
        pendingFire = isPressed;
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