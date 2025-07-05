using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapons/Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float projectileLifetime;
    public float damage;
    public float fireRate;

    [Header("Heat Settings")]
    public float heatPerShot;
    public float maxHeat;
    public float cooldownRate;

    //[Header("Ammo Settings")]
    //public int maxAmmo;
    //public float reloadTime;
}
