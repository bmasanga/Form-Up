using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour, IMoveable
{
    [SerializeField] private float moveSpeed = 1f;

    private Rigidbody2D rb;
    private Vector2 currentInput = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 input)
    {
        currentInput = input;
    }

    private void FixedUpdate()
    {
        if (currentInput.sqrMagnitude <= 0.001f)
            return;

        // Convert input from local to world space based on character facing
        Vector2 relativeInput = transform.right * currentInput.x + transform.up * currentInput.y;

        // Apply force based on raw input magnitude
        Vector2 force = relativeInput * moveSpeed;
        rb.AddForce(force, ForceMode2D.Force);
    }
}



    
