using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] int maxAmmo = 3;
    [SerializeField] int currentAmmo;
    [SerializeField] float missileLifetime = 5f; 

    [SerializeField] float maxReloadTime = 10f;
    [SerializeField] float reloadRate = 1f;
    [SerializeField] float currentReloadTime;

    Agent agent;
    Coroutine launchCoroutine;
    TargetingSystem targetingSystem;

    bool isActioning;
    bool isReloading = false;
    bool isAmmoEmpty = false;

    void Awake()
    {
        agent = GetComponentInParent<Agent>();
        currentAmmo = maxAmmo;
        targetingSystem = GetComponentInChildren<TargetingSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null)
        {
            isActioning = agent.GetisActioning();
        }
        if (isActioning 
        && launchCoroutine == null 
        && !isReloading 
        && targetingSystem != null 
        && targetingSystem.GetLockedTarget() != null)
        {
            Debug.Log("Attempting to launch missile");
            launchCoroutine = StartCoroutine(Launch(targetingSystem.GetLockedTarget()));
        }
    }


    IEnumerator Launch(GameObject target)
    {
        if (currentAmmo > 0)
        {
            currentAmmo --;
            GameObject missileInstance = Instantiate(missilePrefab, transform.position, transform.rotation);
            HomingProjectile missile = missileInstance.GetComponent<HomingProjectile>();
            if (missile != null)
            {
                missile.SetTarget(target);
                Debug.Log("Missile launched at target: " + target.name);

            }
        
            Destroy(missileInstance, missileLifetime);

            isReloading = true;
            currentReloadTime = 0f;

            while (currentReloadTime < maxReloadTime)
            {
                currentReloadTime += reloadRate * Time.deltaTime;
                yield return null;
            }
            
            isReloading = false;
            launchCoroutine = null;

            if(currentAmmo <= 0)
            {
                isAmmoEmpty = true;
            }
        }
        targetingSystem.ResetLockedTarget();
    }


    public float GetCurrentReloadTime()
    {
        return currentReloadTime;
    }

    public float GetMaxReloadTime()
    {
        return maxReloadTime;
    }

    public float GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public bool GetIsReloading()
    {
        return isReloading;
    }
    
    public bool GetIsAmmoEmpty()
    {
        return isAmmoEmpty;
    }
}
