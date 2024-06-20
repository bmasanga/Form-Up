using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // [SerializeField] bool isPlayer;
    [SerializeField] float maxHealth = 50f;
    [SerializeField] float currentHealth;
    [SerializeField] ParticleSystem hitEffect;
   // [SerializeField] int score = 10;

    [SerializeField] Shield shield; 

    [SerializeField] bool applyCameraShake;
    CameraController cameraController;
    

    // AudioPlayer audioPlayer;

    // ScoreKeeper scoreKeeper;

    // LevelManager levelManager;

    void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
        // audioPlayer = FindObjectOfType<AudioPlayer>();
        // scoreKeeper = FindObjectOfType<ScoreKeeper>();
        // levelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();

        float residualDamage = shield.GetResidualDamage();
        bool shieldIsActive = shield.GetisActive();

        if (residualDamage > 0f)
        {
            TakeDamage(shield.GetResidualDamage());
            shield.ResetResidualDamage();
            ShakeCamera();
        }
        else if (!shieldIsActive)
        {
            TakeDamage(damageDealer.GetDamage());
            ShakeCamera();
        }
        
        PlayHitEffect();
        // audioPlayer.PlayDamageClip();
        damageDealer.Hit();
        
        //Debug.Log("Health: " + currentHealth);
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        // if(!isPlayer)
        // {
        //     scoreKeeper.ModifyScore(score);
        // }
        // else
        // {
        //     levelManager.LoadGameOver();
        // }

        Destroy(gameObject);
    }

    void PlayHitEffect()
    {
        if(hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(instance.gameObject, 2f);
        }
    }

    void ShakeCamera()
    {
        if(cameraController != null && applyCameraShake)
        {
            cameraController.ShakeCamera();
            //Debug.Log("Shaking!");
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
