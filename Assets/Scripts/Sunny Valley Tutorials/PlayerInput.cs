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

    [SerializeField] InputActionReference move, look, fire; 

    private void OnEnable()
    {
        move.action.performed += OnMovePerformed;
        move.action.canceled += OnMoveCanceled;

        look.action.performed += OnLookPerformed;

        fire.action.performed += OnFirePerformed;
        fire.action.canceled += OnFireCanceled;

    } 

    private void OnDisable()
    {
        move.action.performed -= OnMovePerformed;
        move.action.canceled -= OnMoveCanceled;

        look.action.performed -= OnLookPerformed;

        fire.action.performed -= OnFirePerformed;
        fire.action.canceled -= OnFireCanceled;
    }
    
    void Update()
    {
        // OnMoveInput?.Invoke(move.action.ReadValue<Vector2>().normalized);
        // OnLookInput?.Invoke(OnLookPerformed());
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
}
