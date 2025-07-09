using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponReadable
{
    float GetCurrentHeat();        
    float GetMaxHeat();            
    bool IsOverheated();         
}
