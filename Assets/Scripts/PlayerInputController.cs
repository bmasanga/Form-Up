using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private IInputReceivable inputReceiver;

    public void Initialize(IInputReceivable receiver)
    {
        inputReceiver = receiver;
        playerInputActions.Player.Enable();
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        var player = playerInputActions.Player;

        player.Move.performed += OnMovePerformed;
        player.Move.canceled += OnMoveCanceled;

        player.Look.performed += OnLookPerformed;
        player.Look.canceled += OnLookCanceled;

        player.Fire.started += OnFireStarted;
        player.Fire.canceled += OnFireCanceled;

        player.Action.started += OnActionStarted;
        player.Action.canceled += OnActionCanceled;

        player.Toggle.started += OnToggleStarted;
        player.Toggle.canceled += OnToggleCanceled;

        player.Target.started += OnTargetStarted;
        player.Target.canceled += OnTargetCanceled;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid duplicate callbacks on re-enable
        var player = playerInputActions.Player;

        player.Move.performed -= OnMovePerformed;
        player.Move.canceled -= OnMoveCanceled;

        player.Look.performed -= OnLookPerformed;
        player.Look.canceled -= OnLookCanceled;

        player.Fire.started -= OnFireStarted;
        player.Fire.canceled -= OnFireCanceled;

        player.Action.started -= OnActionStarted;
        player.Action.canceled -= OnActionCanceled;

        player.Toggle.started -= OnToggleStarted;
        player.Toggle.canceled -= OnToggleCanceled;

        player.Target.started -= OnTargetStarted;
        player.Target.canceled -= OnTargetCanceled;

        // Disable the map so non-owners / disabled objects stop reading input
        playerInputActions.Player.Disable();
    }
    
    private void OnDestroy()
    {
        playerInputActions?.Dispose();
    }

    // --- Handlers ---
    private void OnMovePerformed(InputAction.CallbackContext ctx)
        => inputReceiver?.SetMoveInput(ctx.ReadValue<Vector2>());

    private void OnMoveCanceled(InputAction.CallbackContext _)
        => inputReceiver?.SetMoveInput(Vector2.zero);

    private void OnLookPerformed(InputAction.CallbackContext ctx)
        => inputReceiver?.SetLookInput(ctx.ReadValue<Vector2>());

    private void OnLookCanceled(InputAction.CallbackContext _)
        => inputReceiver?.SetLookInput(Vector2.zero);

    private void OnFireStarted(InputAction.CallbackContext _)
        => inputReceiver?.SetFire(true);

    private void OnFireCanceled(InputAction.CallbackContext _)
        => inputReceiver?.SetFire(false);

    private void OnActionStarted(InputAction.CallbackContext _)
        => inputReceiver?.SetAction(true);

    private void OnActionCanceled(InputAction.CallbackContext _)
        => inputReceiver?.SetAction(false);

    private void OnToggleStarted(InputAction.CallbackContext _)
        => inputReceiver?.SetToggle(true);

    private void OnToggleCanceled(InputAction.CallbackContext _)
        => inputReceiver?.SetToggle(false);

    private void OnTargetStarted(InputAction.CallbackContext _)
        => inputReceiver?.SetTarget(true);

    private void OnTargetCanceled(InputAction.CallbackContext _)
        => inputReceiver?.SetTarget(false);

}