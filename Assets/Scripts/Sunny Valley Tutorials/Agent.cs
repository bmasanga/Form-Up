using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
    [SerializeField] InputActionReference move, look, fire;
    [SerializeField] float rotationSpeed = 200f;
    
    [Header("Shooter")]
    [SerializeField] Shooter shooter;
    
    AgentMover agentMover;

    Vector2 lookInput, moveInput;

    bool isAlive = true;


    private void Awake()
    {
        agentMover = GetComponent<AgentMover>();
    }
    
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
    }

    private void Update()
    {
        //RotateTowardsMouse();
        moveInput = move.action.ReadValue<Vector2>().normalized;
        lookInput = GetLookInput();

        agentMover.moveInput = moveInput;
        // AnimateCharacter();
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log("moveInput: " + moveInput);
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    // void OnLookPerformed(InputAction.CallbackContext context)
    // {
    //     lookInput = context.ReadValue<Vector2>();
    //     //Debug.Log("lookInput: " + lookInput);
    // }

    // void RotateTowardsMouse()
    // {
    //     Vector3 mouseScreenPosition = new Vector3(lookInput.x, lookInput.y, Camera.main.nearClipPlane);
    //     Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    //     Vector3 direction = mouseWorldPosition - transform.position;
    //     direction.z = 0f;

    //     if (direction.magnitude > 0.1f)
    //     {
    //         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    //         Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
    //         transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //     }
    // }

    Vector2 GetLookInput()
    {
        Vector3 mouseScreenPosition = look.action.ReadValue<Vector2>();
        mouseScreenPosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    void RotateToLookInput()
    {
        Vector2 lookDirection = lookInput - (Vector2)transform.position;
        
    }


    void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (!isAlive) return;
        shooter.isFiring = context.ReadValue<float>() > 0;
        //Debug.Log("firing");
    }

    void OnFireCanceled(InputAction.CallbackContext context)
    {
        shooter.isFiring = false;
    }
}