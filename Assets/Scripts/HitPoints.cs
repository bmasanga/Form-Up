using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitPoints : MonoBehaviour, IHitPoints, IHitPointsReadable
{
    [SerializeField] private float currentHP;
    [SerializeField] private float maxHP = 100f;

    public event Action OnDeath;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHP -= damageAmount;
        currentHP = Mathf.Max(currentHP, 0f);

        if (IsDead())
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (OnDeath != null)
        {
            OnDeath.Invoke();
        }

        Destroy(gameObject);
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }

    public bool IsDead()
    {
        return currentHP <= 0f;
    }
}