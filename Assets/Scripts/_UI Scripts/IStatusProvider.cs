using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusProvider
{
    float GetCurrentValue();
    float GetMaxValue();
}
