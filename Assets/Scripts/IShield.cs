using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShield : IDamageable
{
    bool IsActive();
    float GetCurrentShieldHP();
    float GetMaxShieldHP();
}
