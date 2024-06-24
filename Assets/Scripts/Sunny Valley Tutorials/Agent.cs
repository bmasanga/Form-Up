using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 200f;

    [Header("Shooter")]
    [SerializeField] Shooter shooter;

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

    bool isAlive = true;

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

    public void OnFirePerformed()
    {
        if (!isAlive) return;
        shooter.isFiring = true;
    }

    public void OnFireCanceled()
    {
        shooter.isFiring = false;
    }
}
