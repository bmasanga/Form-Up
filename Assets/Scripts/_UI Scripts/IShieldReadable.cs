using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShieldReadable
{
    float GetCurrentShieldHP();
    float GetMaxShieldHP();
    bool IsActive();
}
