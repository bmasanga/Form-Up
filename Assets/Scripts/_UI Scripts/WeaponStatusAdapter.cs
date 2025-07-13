using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatusAdapter : MonoBehaviour, IStatusProvider
{
    private IWeaponReadable weapon;

    void Awake()
    {
        weapon = transform.root.GetComponentInChildren<IWeaponReadable>();
        //Debug.Log("Weapon found: " + (weapon != null), this);

    }

    public float GetCurrentValue() => weapon != null ? weapon.GetCurrentHeat() : 0f;
    public float GetMaxValue() => weapon != null ? weapon.GetMaxHeat() : 1f;
}
