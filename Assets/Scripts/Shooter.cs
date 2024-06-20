
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 1f;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.2f;

    [Header("Cooldown Settings")]
    [SerializeField] bool weaponActive = true;
    [SerializeField] float maxHeat = 10f;
    [SerializeField] float heatPerShot = 1f;
    [SerializeField] float normalCooldownRate = 1f;
    [SerializeField] float overheatedCooldownRate = .5f;

    [SerializeField] float currentHeat = 0f;
    [SerializeField] bool overheated = false;

    [Header("AI")]
    [SerializeField] bool autoFire;

    [HideInInspector] public bool isFiring;

    Coroutine firingCoroutine;
    //AudioPlayer audioPlayer;

    void Awake()
    {
        //audioPlayer = FindObjectOfType<AudioPlayer>();
    }
    
    void Start()
    {
        if (autoFire)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        DissipateHeat();
    }
    
    void FixedUpdate()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

IEnumerator FireContinously()
    {
        while (weaponActive)
        {
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Rigidbody2D rigidBody = projectileInstance.GetComponent<Rigidbody2D>();
            AddHeat();
            //Debug.Log("currentHeat: " + currentHeat);
            if (rigidBody != null)
            {
                // Calculate the force vector
                Vector2 force = transform.up * projectileSpeed;

                // Apply the impulse force to the bullet's Rigidbody2D
                rigidBody.AddForce(force, ForceMode2D.Impulse);
            }
            
            Destroy(projectileInstance, projectileLifetime);
            float timeToNextProjectile = Random.Range(baseFiringRate - firingRateVariance, baseFiringRate + firingRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            yield return new WaitForSecondsRealtime(timeToNextProjectile);
        }
        
    }

    public float GetCurrentHeat()
    {
        return currentHeat;
    }

    public float GetMaxHeat()
    {
        return maxHeat;
    }
    
    void AddHeat()
    {
        currentHeat += heatPerShot;
        currentHeat = Mathf.Min(currentHeat, maxHeat);
        if (currentHeat == maxHeat)
        {
            overheated = true;
            weaponActive = false;
        }
    }

    void DissipateHeat()
    {
        if (currentHeat > 0)
        {
            if (overheated)
            {
                currentHeat -= overheatedCooldownRate * Time.deltaTime;
            }
            else
            {
                currentHeat -= normalCooldownRate * Time.deltaTime;
            }

            currentHeat = Mathf.Max(currentHeat, 0f);
            
        }
        // Reactivate the weapon if it has cooled down below a certain threshold
        if (overheated && currentHeat <= 0)
        {
            weaponActive = true;
            overheated = false;
        } 
    }

    public bool GetOverheatedState()
    {
        return overheated;
    }
}
