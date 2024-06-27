using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] float forwardThrustSpeed = 1.0f;
    [SerializeField] float backwardThrustSpeed = 1.0f;
    [SerializeField] float rightThrustSpeed = 1.0f;
    [SerializeField] float leftThrustSpeed = 1.0f;
    [SerializeField] float rotationSpeed = 5f;

    [Header("Movement Effects")]
    [SerializeField] ParticleSystem fowardThrustEffect;
    [SerializeField] ParticleSystem backwardThrustEffect1;
    [SerializeField] ParticleSystem backwardThrustEffect2;
    [SerializeField] ParticleSystem rightThrustEffect;
    [SerializeField] ParticleSystem leftThrustEffect;

    [Header("Shooter")]
    [SerializeField] Cannon cannon;

    FormUpInputActions controls;
    Rigidbody2D playerRigidBody;
    bool isAlive = true;
    Vector2 moveInput;
    Vector2 lookInput;
    Animator playerAnimator;

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        controls = new FormUpInputActions();
        playerAnimator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        controls.Enable();

        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCancelled;

        controls.Player.Look.performed += OnLookPerformed;

        controls.Player.Fire.performed += OnFirePerformed;
        controls.Player.Fire.canceled += OnFireCancelled;
    }

    void OnDisable()
    {
        controls.Disable();

        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCancelled;

        controls.Player.Look.performed -= OnLookPerformed;

        controls.Player.Fire.performed -= OnFirePerformed;
        controls.Player.Fire.canceled -= OnFireCancelled;
    }

    void Update()
    {
        RotateTowardsMouse();
    }

    void FixedUpdate()
    {
        Thrust();
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log("moveInput: " + moveInput);
    }

    void OnMoveCancelled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        //Debug.Log("lookInput: " + lookInput);
    }

    void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (!isAlive) return;
        cannon.isFiring = context.ReadValue<float>() > 0;
        //Debug.Log("firing");
    }

    void OnFireCancelled(InputAction.CallbackContext context)
    {
        cannon.isFiring = false;
    }

    void Thrust()
    {
        if (moveInput.magnitude > 0)
        {
            Vector2 thrustDirection = transform.TransformDirection(moveInput).normalized;
            Vector2 thrustForce = Vector2.zero;
            playerAnimator.SetBool("isThrusting", true);
            //Debug.Log("isThrusting: " + playerAnimator.GetBool("isThrusting"));

            if (moveInput.y > 0)
            {
                thrustForce += Vector2.up * forwardThrustSpeed * moveInput.y;
                fowardThrustEffect.Play();
            }
            if (moveInput.y < 0)
            {
                thrustForce += Vector2.down * backwardThrustSpeed * Mathf.Abs(moveInput.y);
                backwardThrustEffect1.Play();
                backwardThrustEffect2.Play();
            }    
            if (moveInput.x < 0)
            {
                thrustForce += Vector2.left * leftThrustSpeed * Mathf.Abs(moveInput.x);
                leftThrustEffect.Play();
            }
            if (moveInput.x > 0)
            {
                thrustForce += Vector2.right * rightThrustSpeed * moveInput.x;
                rightThrustEffect.Play();
            }

            
            playerRigidBody.AddForce(thrustDirection * thrustForce.magnitude, ForceMode2D.Force);
            
        }
        else
        {
            // Stop all thrust effects if no movement input
            fowardThrustEffect.Stop();
            backwardThrustEffect1.Stop();
            backwardThrustEffect2.Stop();
            leftThrustEffect.Stop();
            rightThrustEffect.Stop();

            playerAnimator.SetBool("isThrusting", false);
        }
    }


    void RotateTowardsMouse()
    {
        Vector3 mouseScreenPosition = new Vector3(lookInput.x, lookInput.y, Camera.main.nearClipPlane);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0f;

    // Debug.Log("lookInput: " + lookInput);
    // Debug.Log("mouseScreenPosition: " + mouseScreenPosition);
    // Debug.Log("mouseWorldPosition: " + mouseWorldPosition);


        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
