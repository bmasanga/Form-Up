using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStatusAdapter : MonoBehaviour, IStatusProvider
{
    private IShieldReadable shield;

    void Awake()
    {
        shield = transform.root.GetComponentInChildren<IShieldReadable>();
        //Debug.Log("Shield found: " + (shield != null), this);

    }

    public float GetCurrentValue() => shield != null ? shield.GetCurrentShieldHP() : 0f;
    public float GetMaxValue() => shield != null ? shield.GetMaxShieldHP() : 1f;
}
