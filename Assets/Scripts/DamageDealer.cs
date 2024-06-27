using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float damage = 10;
    [SerializeField] ParticleSystem hitEffect;

    void OnCollisionEnter2D(Collision2D other) 
    {   
        PlayHitEffect(other.contacts[0].point);

        Shield shield = other.gameObject.GetComponentInChildren<Shield>();
        if (shield != null)
        {
            shield.TakeDamage(damage);
        }
    }

    void PlayHitEffect(Vector2 collisionPoint)
    {
        if(hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, collisionPoint, transform.rotation);
            Destroy(instance.gameObject, 2f);
        }
    }
}
