using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{

    [SerializeField] private float damage = 10f;
    [SerializeField] private ParticleSystem hitEffect;
    [Tooltip("If true, destroy this GameObject on the first successful hit.")]
    [SerializeField] private bool destroyOnHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ApplyDamage(collision.collider, collision.GetContact(0).point);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ApplyDamage(other, transform.position);
    }

    private void ApplyDamage(Collider2D target, Vector2 hitPoint)
    {
        IDamageable[] damageables = target.GetComponentsInChildren<IDamageable>();

        for (int i = 0; i < damageables.Length; i++)
        {
            if (damageables[i] is IShield)
            {
                damageables[i].TakeDamage(damage);
                PlayHitEffect(hitPoint);
                TryDestroySelf();
                return;
            }
        }

        for (int i = 0; i < damageables.Length; i++)
        {
            if (damageables[i] is IHitPoints)
            {
                damageables[i].TakeDamage(damage);
                PlayHitEffect(hitPoint);
                TryDestroySelf();
                return;
            }
        }
    }

    private void PlayHitEffect(Vector2 point)
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, point, Quaternion.identity);
            Destroy(instance.gameObject, 2f);
        }
    }
    private void TryDestroySelf()
    {
        if (!destroyOnHit) return;
        // disable collider so you don’t get double-hits
        var colider = GetComponent<Collider2D>();
        if (colider) colider.enabled = false;
        Destroy(gameObject);
    }

/*
        [SerializeField] private float damage = 10f;
        [SerializeField] private ParticleSystem hitEffect;
        [Tooltip("If true, destroy this GameObject on the first successful hit.")]
        [SerializeField] private bool destroyOnHit = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HandleHit(collision.collider, collision.GetContact(0).point);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            HandleHit(other, transform.position);
        }

        private void HandleHit(Collider2D target, Vector2 hitPoint)
        {
            // 1) Spawn VFX
            PlayHitEffect(hitPoint);

            // 2) Try to hit a shield first
            var damageables = target.GetComponentsInChildren<IDamageable>();
            foreach (var d in damageables)
            {
                if (d is IShield)
                {
                    d.TakeDamage(damage);
                    TryDestroySelf();
                    return;
                }
            }

            // 3) Otherwise hit HP
            foreach (var d in damageables)
            {
                if (d is IHitPoints)
                {
                    d.TakeDamage(damage);
                    TryDestroySelf();
                    return;
                }
            }

            // 4) If you still want the thing to vanish when hitting non-damageables:
            TryDestroySelf();
        }

        private void PlayHitEffect(Vector2 point)
        {
            if (hitEffect == null) return;
            var inst = Instantiate(hitEffect, point, Quaternion.identity);
            Destroy(inst.gameObject, 2f);
        }

        private void TryDestroySelf()
        {
            if (!destroyOnHit) return;
            // disable collider so you don’t get double-hits
            var col = GetComponent<Collider2D>();
            if (col) col.enabled = false;
            Destroy(gameObject);
        }
    */
}