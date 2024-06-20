using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
    // [SerializeField] InputActionReference move, look;
    [SerializeField] float rotationSpeed = 200f;
    
    [Header("Shooter")]
    [SerializeField] Shooter shooter;
    
    AgentMover agentMover;

    Vector2 _lookInput, _moveInput;

    bool isAlive = true;

    public Vector2 lookInput 
    { 
        get => _lookInput; 
        set => _lookInput = value; 
    }
    
    public Vector2 moveInput 
    { 
        get => _moveInput; 
        set => _moveInput = value; }


    private void Awake()
    {
        agentMover = GetComponent<AgentMover>();
    }
    
    private void OnEnable()
    {
        // move.action.performed += OnMovePerformed;
        // move.action.canceled += OnMoveCanceled;

        // look.action.performed += OnLookPerformed;

        // fire.action.performed += OnFirePerformed;
        // fire.action.canceled += OnFireCanceled;

        // Subscribing to events from PlayerInput
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.OnMoveInput.AddListener(OnMovePerformed);
            playerInput.OnLookInput.AddListener(OnLookPerformed);
            playerInput.OnFireInput.AddListener(OnFirePerformed);
            playerInput.OnFireCancel.AddListener(OnFireCanceled);
        }
    }

    private void OnDisable()
    {
        // move.action.performed -= OnMovePerformed;
        // move.action.canceled -= OnMoveCanceled;

        // look.action.performed -= OnLookPerformed;

        // fire.action.performed -= OnFirePerformed;
        // fire.action.canceled -= OnFireCanceled;
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.OnMoveInput.RemoveListener(OnMovePerformed);
            playerInput.OnLookInput.RemoveListener(OnLookPerformed);
            playerInput.OnFireInput.RemoveListener(OnFirePerformed);
            playerInput.OnFireCancel.RemoveListener(OnFireCanceled);
        }
    }

    private void Update()
    {
        // lookInput = CalcMouseScreenPosition();
        // moveInput = move.action.ReadValue<Vector2>().normalized;

        agentMover.moveInput = moveInput;
        RotateTowardPointer();
    }

    public void OnMovePerformed(Vector2 input)
    {
        moveInput = input;
        //Debug.Log("moveInput: " + moveInput);
    }

    // void OnMoveCanceled(InputAction.CallbackContext context)
    // {
    //     moveInput = Vector2.zero;
    // }

    public void OnLookPerformed(Vector2 input)
    {
        lookInput = input;
        //Debug.Log("lookInput: " + lookInput);
    }

    void RotateTowardPointer()
    {
        Vector3 mouseScreenPosition = new Vector3(lookInput.x, lookInput.y, Camera.main.nearClipPlane);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0f;

        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    Vector2 CalcMouseScreenPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(lookInput.x, lookInput.y, Camera.main.nearClipPlane);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    public void OnFirePerformed()
    {
        if (!isAlive) return;
        shooter.isFiring = true;
        //Debug.Log("firing");
    }

    public void OnFireCanceled()
    {
        shooter.isFiring = false;
    }
}