using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField] float maxSpeed = 2, acceleration = 50, deceleration = 100;
    [SerializeField] float currentSpeed = 0;
    private Vector2 oldMovementInput;

    [SerializeField] float thrustSpeed = 1.0f;
    //[SerializeField] float decelerationRate = 5.0f;
    // Vector2 thrustDirection;
    // Vector2 thrustForce;

    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //Move();
        Thrust();
        //Thrust2();

    }

    // private void Move()
    // {
    //     if (MovementInput.magnitude > 0 && currentSpeed >= 0)
    //     {
    //         oldMovementInput = MovementInput;

    //         currentSpeed += acceleration * maxSpeed * Time.deltaTime;
    //     }
    //     else
    //     {
    //         currentSpeed -= deceleration * maxSpeed * Time.deltaTime;
    //     }
    //     currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    //     rb2d.velocity = oldMovementInput * currentSpeed;
    // }

    private void Thrust()
    {
         if (MovementInput.magnitude > 0)
        {
            rb2d.AddForce(MovementInput.normalized * thrustSpeed * MovementInput.magnitude, ForceMode2D.Force);

            
            // //Convert the MovementInput from global space to local space
            // Vector2 localDirection = transform.InverseTransformDirection(MovementInput).normalized;
        
            // // Apply force in the local direction
            // rb2d.AddForce(localDirection * thrustSpeed * MovementInput.magnitude, ForceMode2D.Force);
            
            // Vector2 thrustDirection = transform.TransformDirection(MovementInput).normalized;
            // rb2d.AddForce(thrustDirection * thrustSpeed * MovementInput.magnitude, ForceMode2D.Force);

        }
    }

    // private void Thrust2()
    // {
    //     if (MovementInput.magnitude > 0)
    //     {
    //         // Save the current movement input
    //         oldMovementInput = -MovementInput;

    //         // Apply force in the direction of the input
    //         thrustDirection = transform.TransformDirection(oldMovementInput).normalized;
    //         rb2d.AddForce(thrustDirection * thrustSpeed * MovementInput.magnitude, ForceMode2D.Force);
    //     }
    //     else
    //     {
    //         // Apply deceleration force in the opposite direction of the current velocity
    //         if (rb2d.velocity.magnitude > 0)
    //         {
    //             Vector2 decelerationDirection = -rb2d.velocity.normalized;
    //             rb2d.AddForce(decelerationDirection * deceleration, ForceMode2D.Force);
    //         }
    //     }
    // }            

}