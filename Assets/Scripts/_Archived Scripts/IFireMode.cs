using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireMode
{
    void Initialize(IWeapon weapon);
    void HandleFire();
    bool CanFire();
}
