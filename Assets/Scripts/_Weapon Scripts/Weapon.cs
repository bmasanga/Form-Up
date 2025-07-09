using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon, IWeaponReadable
{
    [SerializeField] protected WeaponConfig config;

    private float lastFireTime;
    private float currentHeat;
    private bool isOverheated = false;

    public WeaponConfig GetConfig() => config;

    private void Update()
    {
        CoolDownHeat();
    }

    public void Fire()
    {
        if (!CanFire()) return;

        HandleFire();
        lastFireTime = Time.time;
        AddHeat();
    }

    protected virtual void HandleFire()
    {
        GameObject projectile = Instantiate(config.projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            //rb.velocity = transform.up * config.projectileSpeed;
            // Calculate the force vector
            Vector2 force = transform.up * config.projectileSpeed;

            // Apply the impulse force to the bullet's Rigidbody2D
            rb.AddForce(force, ForceMode2D.Impulse);
            Destroy(projectile, config.projectileLifetime);

        }
    }

    private bool CanFire()
    {
        float secondsBetweenShots = 1f / config.fireRate;
        return Time.time >= lastFireTime + secondsBetweenShots && !isOverheated;
    }

    private void AddHeat()
    {
        currentHeat += config.heatPerShot;
        if (currentHeat >= config.maxHeat)
        {
            isOverheated = true;
        }
    }

    private void CoolDownHeat()
    {
        if (currentHeat > 0)
        {
            currentHeat -= config.cooldownRate * Time.deltaTime;
            if (currentHeat <= 0)
            {
                currentHeat = 0;
                isOverheated = false;
            }
        }
    }

    public float GetCurrentHeat()
    {
        return currentHeat;
    }
    public float GetMaxHeat()
    {
        return config.maxHeat;
    }
    public bool IsOverheated()
    {
        return isOverheated;
    }     

}