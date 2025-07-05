using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    // public UnityEvent<Vector2> OnMoveInput, OnLookInput;
    public UnityEvent<Vector2> OnMoveInput;
    public UnityEvent<Vector2> OnLookInput;
    public UnityEvent OnFireInput;
    public UnityEvent OnFireCancel;
    public UnityEvent OnActionInput;
    public UnityEvent OnActionCancel;
    public UnityEvent OnToggleInput;
    public UnityEvent OnToggleCancel;
    public UnityEvent OnTargetInput;
    public UnityEvent OnTargetCancel;


    [SerializeField] InputActionReference move, look, fire, action, toggle, target; 

    private void OnEnable()
    {
        move.action.performed += OnMovePerformed;
        move.action.canceled += OnMoveCanceled;

        look.action.performed += OnLookPerformed;

        fire.action.performed += OnFirePerformed;
        fire.action.canceled += OnFireCanceled;

        action.action.performed += OnActionPerformed;
        action.action.canceled += OnActionCanceled;

        toggle.action.performed += OnTogglePerformed;
        toggle.action.canceled += OnToggleCanceled;
        
        target.action.performed += OnTargetPerformed;
        target.action.canceled += OnTargetCanceled;
    } 

    private void OnDisable()
    {
        move.action.performed -= OnMovePerformed;
        move.action.canceled -= OnMoveCanceled;

        look.action.performed -= OnLookPerformed;

        fire.action.performed -= OnFirePerformed;
        fire.action.canceled -= OnFireCanceled;

        action.action.performed -= OnActionPerformed;
        action.action.canceled -= OnActionCanceled;

        toggle.action.performed -= OnTogglePerformed;
        toggle.action.canceled -= OnToggleCanceled;

        target.action.performed -= OnTargetPerformed;
        target.action.canceled -= OnTargetCanceled;
    }
    

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(Vector2.zero);
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        OnLookInput?.Invoke(context.ReadValue<Vector2>());

    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        OnFireInput?.Invoke();
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        OnFireCancel?.Invoke();
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        OnActionInput?.Invoke();
    }

    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        OnActionCancel?.Invoke();
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        OnToggleInput?.Invoke();
    }

    private void OnToggleCanceled(InputAction.CallbackContext context)
    {
        OnToggleCancel?.Invoke();
    }

    private void OnTargetPerformed(InputAction.CallbackContext context)
    {
        OnTargetInput?.Invoke();
    }

    private void OnTargetCanceled(InputAction.CallbackContext context)
    {
        OnTargetCancel?.Invoke();
    }
}
