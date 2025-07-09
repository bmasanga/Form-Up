using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHitPoints : IDamageable
{
    float GetCurrentHP();
    float GetMaxHP();
    bool IsDead();
    event Action OnDeath;
}
