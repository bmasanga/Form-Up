using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDamageDealer : MonoBehaviour
{
    [SerializeField] float damage = 10;
    [SerializeField] ParticleSystem hitEffect;

    private void OnCollisionEnter2D(Collision2D other)
    {
        HandleDamage(other.collider, other.GetContact(0).point);
    }

    private void HandleDamage(Collider2D target, Vector2 collisionPoint)
    {
        // 1) Spawn VFX
        PlayHitEffect(collisionPoint);

        // 2) Try to hit a shield first
        var damageables = target.GetComponentsInChildren<IDamageable>();
        foreach (var d in damageables)
        {
            if (d is IShield)
            {
                d.TakeDamage(damage);
                return;
            }
        }

        // 3) Otherwise hit HP
        foreach (var d in damageables)
        {
            if (d is IHitPoints)
            {
                d.TakeDamage(damage);
                return;
            }
        }
    }

    private void PlayHitEffect(Vector2 collisionPoint)
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, collisionPoint, transform.rotation);
            Destroy(instance.gameObject, 2f);
        }
    }
}
