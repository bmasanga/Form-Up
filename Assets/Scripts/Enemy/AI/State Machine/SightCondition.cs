using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SightCondition", menuName = "Conditions/SightCondition")]
public class SightCondition : Condition
{
    public override bool Test(AIData aIData)
    {
        return aIData.targets != null;
    }
}
