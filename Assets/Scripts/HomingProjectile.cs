using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] new ParticleSystem particleSystem;
    [SerializeField] float speed = 5f;
    [SerializeField] float rotateSpeed = 200f;
    [SerializeField] float damage = 100f;

    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (target == null) return;
        
        Vector2 direction = (Vector2)target.position - rb2d.position;
        
        direction.Normalize();
        
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        
        rb2d.angularVelocity = -rotateAmount * rotateSpeed;

        rb2d.velocity = transform.up * speed;
    }

    public void SetTarget(GameObject lockedTarget)
    {
        if (lockedTarget != null)
        {
        target = lockedTarget.transform;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Shield shield = other.GetComponent<Shield>();
        if (shield != null)
        {
            shield.TakeDamage(damage);
        }

        HandleExplosion(0);
    }

    void HandleExplosion(float delay)
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
