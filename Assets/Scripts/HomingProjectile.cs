using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 5f;
    [SerializeField] float rotateSpeed = 200f;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] new ParticleSystem particleSystem;

    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //target = GameObject.FindGameObjectWithTag("MissileTarget").transform;
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb2d.position;
        
        direction.Normalize();
        
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        
        rb2d.angularVelocity = -rotateAmount * rotateSpeed;

        rb2d.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        DetachParticles();
        Destroy(gameObject);
    }

    void DetachParticles()
    {
        // Detach the particle system from the parent
        particleSystem.transform.parent = null;

        // Make sure the particle system's StopAction is set to None or Disable
        var mainModule = particleSystem.main;
        mainModule.stopAction = ParticleSystemStopAction.Destroy; 

        // Optionally, stop emitting new particles
        particleSystem.Stop();
    }
}
