using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 50f;
    [SerializeField] float currentHealth;

    [SerializeField] Shield shield; 

    [SerializeField] bool applyCameraShake;
    [SerializeField] bool isPlayer = false;
    [SerializeField] int score = 100;
    CameraController cameraController;
    LevelManager levelManager;

    ScoreKeeper scoreKeeper;


    void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
        levelManager = FindObjectOfType<LevelManager>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
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
       if(!isPlayer)
        {
            scoreKeeper.ModifyScore(score);
        }
        else
        {
            levelManager.LoadGameOver();
        }
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
