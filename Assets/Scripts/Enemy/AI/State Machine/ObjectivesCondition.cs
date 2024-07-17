using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectivesCondition", menuName = "Conditions/ObjectivesCondition")]
public class ObjectivesCondition : Condition
{
    public override bool Test(AIData aIData, EnemyAgent enemyAgent)
    {
        if (aIData.objectives != null)
        {
            return true;
        }
        
        return false;
    }
}
