using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] bool isActive = true;
    [SerializeField] float maxHP = 50f;
    [SerializeField] float currentHP;
    [SerializeField] float regenRate = 10f;
    [SerializeField] float activeDelayTime = 5f;
    [SerializeField] float deactivatedDelayTime = 10f;
    [SerializeField] SpriteRenderer shieldSprite;
    float residualDamage = 0f;

    Collider2D shieldCollider;
    Coroutine regenCoroutine;
    Health health;



    void Awake()
    {
        currentHP = maxHP;
        shieldCollider = GetComponent<Collider2D>();
        health = GetComponentInParent<Health>();
    }

    public void TakeDamage(float damage)
    {
        if (damage > currentHP)
        {
            residualDamage = damage - currentHP;
        } 
        else
        {
            residualDamage = 0f;
        }   
        

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
        
        if (currentHP <= 0f)
        {
            DeactivateShield();
            regenCoroutine = StartCoroutine(RegenShield(deactivatedDelayTime));
        }
        else
        {
            regenCoroutine = StartCoroutine(RegenShield(activeDelayTime));
        }
        
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0f);
        HandleResidualDamage();
    }

    void DeactivateShield()
    {
        isActive = false;
        shieldCollider.enabled = false;
        shieldSprite.enabled = false;
    }

    IEnumerator RegenShield(float delayTime)
    {
        
        yield return new WaitForSecondsRealtime(delayTime);

        ActivateShield();

        while (currentHP < maxHP)
        {
            currentHP += regenRate * Time.deltaTime;
            currentHP = Mathf.Min(currentHP, maxHP);

            yield return null;
        }
        
    }

    void ActivateShield()
    {
        isActive = true;
        shieldCollider.enabled = true;
        shieldSprite.enabled = true;
    }

    void HandleResidualDamage()
    {
        health.TakeDamage(residualDamage);
        ResetResidualDamage();
    }

    
    
    public bool GetisActive()
    {
        return isActive;
    }
    
    public float GetResidualDamage()
    {
        return residualDamage;
    }

    public void ResetResidualDamage()
    {
        residualDamage = 0f;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }
}
