using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class Agent : MonoBehaviour
{
    [SerializeField] float contollerDeadzone = 0.1f;

    [SerializeField] float rotationSpeed = 200f;

    [SerializeField] bool isGamepad;

    bool isFiring = false;
    bool isActioning = false;
    bool isTargeting = false;
    bool isAlive = true;
    bool isToggling = false;

    AgentMover agentMover;

    private Vector2 _lookInput, _moveInput;

    public Vector2 lookInput
    {
        get => _lookInput;
        set => _lookInput = value;
    }

    public Vector2 moveInput
    {
        get => _moveInput;
        set => _moveInput = value;
    }

    private void Awake()
    {
        agentMover = GetComponent<AgentMover>();
    }

    private void OnEnable()
    {
        // Subscribing to events from PlayerInput
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.OnMoveInput.AddListener(OnMovePerformed);
            playerInput.OnLookInput.AddListener(OnLookPerformed);
            playerInput.OnFireInput.AddListener(OnFirePerformed);
            playerInput.OnFireCancel.AddListener(OnFireCanceled);
            playerInput.OnActionInput.AddListener(OnActionPerformed);
            playerInput.OnActionCancel.AddListener(OnActionCanceled);
            playerInput.OnToggleInput.AddListener(OnTogglePerformed);
            playerInput.OnToggleCancel.AddListener(OnToggleCanceled);
            playerInput.OnTargetInput.AddListener(OnTargetPerformed);
            playerInput.OnTargetCancel.AddListener(OnTargetCanceled);
        }
        // Subscribing to events from EnemyAI
        // EnemyAI enemyAI = GetComponent<EnemyAI>();
        // if (enemyAI != null)
        // {
        //     enemyAI.OnMoveInput.AddListener(OnMovePerformed);
        //     enemyAI.OnLookInput.AddListener(OnLookPerformed);
        //     enemyAI.OnFireInput.AddListener(OnFirePerformed);
        //     enemyAI.OnFireCancel.AddListener(OnFireCanceled);
        // }
    }

    private void OnDisable()
    {
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.OnMoveInput.RemoveListener(OnMovePerformed);
            playerInput.OnLookInput.RemoveListener(OnLookPerformed);
            playerInput.OnFireInput.RemoveListener(OnFirePerformed);
            playerInput.OnFireCancel.RemoveListener(OnFireCanceled);
            playerInput.OnActionInput.RemoveListener(OnActionPerformed);
            playerInput.OnActionCancel.RemoveListener(OnActionCanceled);
            playerInput.OnToggleInput.RemoveListener(OnTogglePerformed);
            playerInput.OnToggleCancel.RemoveListener(OnToggleCanceled);
            playerInput.OnTargetInput.RemoveListener(OnTargetPerformed);
            playerInput.OnTargetCancel.RemoveListener(OnTargetCanceled);
        }
        // Unsubscribing from events from EnemyAI
        // EnemyAI enemyAI = GetComponent<EnemyAI>();
        // if (enemyAI != null)
        // {
        //     enemyAI.OnMoveInput.RemoveListener(OnMovePerformed);
        //     enemyAI.OnLookInput.RemoveListener(OnLookPerformed);
        //     enemyAI.OnFireInput.RemoveListener(OnFirePerformed);
        //     enemyAI.OnFireCancel.RemoveListener(OnFireCanceled);
        // }
    }

    private void Update()
    {
        agentMover.moveInput = moveInput;
        RotateTowardPointer();
    }

    public void OnMovePerformed(Vector2 input)
    {
        moveInput = input;
    }

    public void OnLookPerformed(Vector2 input)
    {
        lookInput = input;
    }

    void RotateTowardPointer()
    {
        if (Gamepad.current != null) 
        {
            if (Mathf.Abs(Gamepad.current.rightStick.ReadValue().magnitude) > contollerDeadzone)
            {
                // Use gamepad right stick input for rotation
                Vector2 gamepadInput = Gamepad.current.rightStick.ReadValue();
                float angle = Mathf.Atan2(gamepadInput.y, gamepadInput.x) * Mathf.Rad2Deg - 90f;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
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
    }

    public void OnFirePerformed()
    {
        if (!isAlive) return;
        isFiring = true;
    }

    public void OnFireCanceled()
    {
        isFiring = false;
    }

    public void OnActionPerformed()
    {
        if (!isAlive) return;
        isActioning = true;
    }

    public void OnActionCanceled()
    {
        isActioning = false;
    }

    public void OnTogglePerformed()
    {
        if (!isAlive) return;
        isToggling = true;
    }

    public void OnToggleCanceled()
    {
        isToggling = false;
    }

    public void OnTargetPerformed()
    {
        if (!isAlive) return;
        isTargeting = true;
    }

    public void OnTargetCanceled()
    {
        isTargeting = false;
    }

    public bool GetIsFiring()
    {
        return isFiring;
    }

    public bool GetIsActioning()
    {
        return isActioning;
    }

    public bool GetIsToggling()
    {
        return isToggling;
    }

    public bool GetIsTargeting()
    {
        return isTargeting;
    }
}
