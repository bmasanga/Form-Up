using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageDealer : MonoBehaviour
{
    [SerializeField] float damage = 10;
    [SerializeField] ParticleSystem hitEffect;

    private Collider2D _collider;

    private void Awake()
    {
        // Cache this so you can disable it immediately on hit
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
            Debug.LogError($"[{name}] needs a Collider2D to detect triggers.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1) Prevent any other triggers in this frame
        if (_collider) 
            _collider.enabled = false;

        // 2) Spawn VFX at your current position/orientation
        PlayHitEffect(transform.position);

        // 3) Damage shield then HP
        var damageables = other.GetComponentsInChildren<IDamageable>();
        foreach (var d in damageables)
        {
            if (d is IShield shield && shield.IsActive())
            {
                shield.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }
        foreach (var d in damageables)
        {
            if (d is IHitPoints hp)
            {
                hp.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        // 4) Fallback: destroy on anything else
        Destroy(gameObject);
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
