using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] float forwardThrustSpeed = 1.0f;
    [SerializeField] float backwardThrustSpeed = 1.0f;
    [SerializeField] float rightThrustSpeed = 1.0f;
    [SerializeField] float leftThrustSpeed = 1.0f;
    // [SerializeField] float rotationSpeed = 5f;

    [Header("Movement Effects")]
    [SerializeField] ParticleSystem fowardThrustEffect;
    [SerializeField] ParticleSystem backwardThrustEffect1;
    [SerializeField] ParticleSystem backwardThrustEffect2;
    [SerializeField] ParticleSystem rightThrustEffect;
    [SerializeField] ParticleSystem leftThrustEffect;
    
    private Rigidbody2D rb2d;
    Animator animator;

    public Vector2 moveInput { get; set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Thrust();
    }

    void Thrust()
    {
        if (moveInput.magnitude > 0)
        {
            Vector2 thrustDirection = transform.TransformDirection(moveInput).normalized;
            Vector2 thrustForce = Vector2.zero;
            animator.SetBool("isThrusting", true);
            //Debug.Log("isThrusting: " + playerAnimator.GetBool("isThrusting"));

            if (moveInput.y > 0)
            {
                thrustForce += Vector2.up * forwardThrustSpeed * moveInput.y;
                fowardThrustEffect.Play();
                backwardThrustEffect1.Stop();
                backwardThrustEffect2.Stop();
            }
            if (moveInput.y < 0)
            {
                thrustForce += Vector2.down * backwardThrustSpeed * Mathf.Abs(moveInput.y);
                backwardThrustEffect1.Play();
                backwardThrustEffect2.Play();
                fowardThrustEffect.Stop();
            }    
            if (moveInput.x < 0)
            {
                thrustForce += Vector2.left * leftThrustSpeed * Mathf.Abs(moveInput.x);
                leftThrustEffect.Play();
                rightThrustEffect.Stop();
            }
            if (moveInput.x > 0)
            {
                thrustForce += Vector2.right * rightThrustSpeed * moveInput.x;
                rightThrustEffect.Play();
                leftThrustEffect.Stop();
            }
            if (moveInput.y == 0)
            {
                fowardThrustEffect.Stop();
                backwardThrustEffect1.Stop();
                backwardThrustEffect2.Stop();
            }
            if (moveInput.x == 0)
            {
                rightThrustEffect.Stop();
                leftThrustEffect.Stop();
            }
            

            
            rb2d.AddForce(thrustDirection * thrustForce.magnitude, ForceMode2D.Force);

        }
        else
        {
            // Stop all thrust effects if no movement input
            fowardThrustEffect.Stop();
            backwardThrustEffect1.Stop();
            backwardThrustEffect2.Stop();
            leftThrustEffect.Stop();
            rightThrustEffect.Stop();

            animator.SetBool("isThrusting", false);
        }
    }


}