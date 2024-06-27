using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;

    [SerializeField] bool isAmmoEmpty = false;
    [SerializeField] int maxAmmo = 3;
    [SerializeField] int currentAmmo;
    [SerializeField] float missileLifetime = 5f; 

    [SerializeField] float maxReloadTime = 10f;
    [SerializeField] float reloadRate = 1f;
    [SerializeField] float currentReloadTime;

    Agent agent;
    bool isActioning;
    Coroutine launchCoroutine;
    bool isReloading = false;

    void Awake()
    {
        agent = GetComponentInParent<Agent>();
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null)
        {
            isActioning = agent.GetisActioning();
        }
        if (isActioning && launchCoroutine == null && !isReloading)
        {
            launchCoroutine = StartCoroutine(Launch());
        }
    }

    void FixedUpdate()
    {
        Launch();
    }

    IEnumerator Launch()
    {
        if (currentAmmo > 0)
        {
            currentAmmo --;
            GameObject missileInstance = Instantiate(missilePrefab, transform.position, transform.rotation);
        
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
        }
    }

    void EmptyAmmo()
    {
        if(currentAmmo <= 0)
        {
            isAmmoEmpty = true;
        }
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
