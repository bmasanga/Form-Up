using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LostSightCondition", menuName = "Conditions/LostSightCondition")]
public class LostSightCondition : Condition
{
    public override bool Test(AIData aIData)
    {
        return aIData.currentTarget == null;
    }
}
