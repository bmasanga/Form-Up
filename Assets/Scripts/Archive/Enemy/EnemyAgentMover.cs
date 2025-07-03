using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;

    // [SerializeField] float maxSpeed = 2, acceleration = 50, deceleration = 100;
    // [SerializeField] float currentSpeed = 0;
    // private Vector2 oldMovementInput;

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
        Thrust();
    }



    private void Thrust()
    {
         if (MovementInput.magnitude > 0)
        {
            rb2d.AddForce(MovementInput.normalized * thrustSpeed * MovementInput.magnitude, ForceMode2D.Force);

            
          
    

        }
    }

     // //Convert the MovementInput from global space to local space
    // Vector2 localDirection = transform.InverseTransformDirection(MovementInput).normalized;
    // thrustDirection = transform.TransformDirection(oldMovementInput).normalized;

}