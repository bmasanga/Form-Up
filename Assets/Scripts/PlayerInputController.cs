using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float controllerDeadzone = 0.1f;

    private PlayerInputActions playerInputActions;
    private IInputReceivable inputReceiver;

    public void Initialize(IInputReceivable receiver)
    {
        inputReceiver = receiver;
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();

        playerInputActions.Player.Move.performed += ctx => inputReceiver?.SetMoveInput(ctx.ReadValue<Vector2>());
        playerInputActions.Player.Move.canceled += ctx => inputReceiver?.SetMoveInput(Vector2.zero);

        playerInputActions.Player.Look.performed += OnLookPerformed;
        playerInputActions.Player.Look.canceled += ctx => inputReceiver?.SetLookInput(Vector2.zero, false);

        playerInputActions.Player.Fire.started += ctx => inputReceiver?.SetFire(true);
        playerInputActions.Player.Fire.canceled += ctx => inputReceiver?.SetFire(false);

        playerInputActions.Player.Action.started += ctx => inputReceiver?.SetAction(true);
        playerInputActions.Player.Action.canceled += ctx => inputReceiver?.SetAction(false);

        playerInputActions.Player.Toggle.started += ctx => inputReceiver?.SetToggle(true);
        playerInputActions.Player.Toggle.canceled += ctx => inputReceiver?.SetToggle(false);

        playerInputActions.Player.Target.started += ctx => inputReceiver?.SetTarget(true);
        playerInputActions.Player.Target.canceled += ctx => inputReceiver?.SetTarget(false);
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        bool isGamepad = Gamepad.current != null && input.magnitude > controllerDeadzone;

        // Interpret as stick direction or screen position
        inputReceiver?.SetLookInput(input, isWorldPosition: !isGamepad);
    }
}