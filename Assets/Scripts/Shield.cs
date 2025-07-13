using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour, IShield, IShieldReadable
{
    [SerializeField] private float currentShieldHP;
    [SerializeField] private float regenDelayTimer = 0f;
    [SerializeField] private float maxShieldHP = 50f;
    [SerializeField] private float regenRate = 10f;
    [SerializeField] private float regenDelayAfterHit = 5f;
    [SerializeField] private float regenDelayAfterDepletion = 10f;
    
    private bool isActive = true;
    private bool isWaitingToRegen = false;
    private bool isRegenerating = false;
    
    private SpriteRenderer shieldSprite;
    private Collider2D shieldCollider;
    private IHitPoints hitPoints;

    private void Awake()
    {
        shieldSprite = GetComponentInChildren<SpriteRenderer>();
        shieldCollider = GetComponent<Collider2D>();
        currentShieldHP = maxShieldHP;
        hitPoints = GetComponentInParent<IHitPoints>();

        if (shieldSprite == null)
            Debug.LogError($"[{name}] missing child SpriteRenderer for shield visuals.");
        if (shieldCollider == null)
            Debug.LogError($"[{name}] missing Collider2D to block hits.");
        if (hitPoints == null)
            Debug.LogError($"[{name}] couldn’t find IHitPoints on parent—damage won’t spill over.");
    }

    private void Update()
    {
        if (isWaitingToRegen)
        {
            regenDelayTimer -= Time.deltaTime;
            if (regenDelayTimer <= 0f)
            {
                isWaitingToRegen = false;
                isRegenerating = true;
                regenDelayTimer = 0f;
                ActivateShield();
            }
        }

        if (isRegenerating)
        {
            currentShieldHP += regenRate * Time.deltaTime;

            if (currentShieldHP >= maxShieldHP)
            {
                currentShieldHP = maxShieldHP;
                isRegenerating = false;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (!isActive || currentShieldHP <= 0f)
        {
            PassDamageToHitPoints(damageAmount);
            return;
        }

        float residualDamage = Mathf.Max(damageAmount - currentShieldHP, 0f);

        if (residualDamage > 0f)
        {
            PassDamageToHitPoints(residualDamage);
        }

        currentShieldHP = Mathf.Max(currentShieldHP - damageAmount, 0f);

        if (currentShieldHP <= 0f)
        {
            DeactivateShield();
            StartRegenDelay(regenDelayAfterDepletion);
        }
        else
        {
            StartRegenDelay(regenDelayAfterHit);
        }
    }

    private void PassDamageToHitPoints(float damage)
    {
        if (hitPoints != null)
        {
            hitPoints.TakeDamage(damage);
        }
    }

    private void StartRegenDelay(float delay)
    {
        isWaitingToRegen = true;
        isRegenerating = false;
        regenDelayTimer = delay;
    }

    private void DeactivateShield()
    {
        isActive = false;
        isWaitingToRegen = false;
        isRegenerating = false;
        if (shieldSprite != null) shieldSprite.enabled = false;
        if (shieldCollider) shieldCollider.enabled = false;  // let hits pass through

    }

    private void ActivateShield()
    {
        isActive = true;
        if (shieldSprite != null) shieldSprite.enabled = true;
        if (shieldCollider) shieldCollider.enabled = true;        
    }

    public bool IsActive()
    {
        return isActive;
    }

    public float GetCurrentShieldHP()
    {
        return currentShieldHP;
    }

    public float GetMaxShieldHP()
    {
        return maxShieldHP;
    }
}
