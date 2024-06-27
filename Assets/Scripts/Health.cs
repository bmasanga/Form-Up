using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 50f;
    [SerializeField] float currentHealth;

    [SerializeField] Shield shield; 

    [SerializeField] bool applyCameraShake;
    CameraController cameraController;


    void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
        {
            Die();
        }
        
        ShakeCamera();
    }

    void Die()
    {
        Destroy(gameObject);
    }


    void ShakeCamera()
    {
        if(cameraController != null && applyCameraShake)
        {
            cameraController.ShakeCamera();
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
