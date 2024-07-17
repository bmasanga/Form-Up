using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoObjectivesCondition", menuName = "Conditions/NoObjectivesCondition")]
public class NoObjectivesCondition : Condition
{
    public override bool Test(AIData aIData, EnemyAgent enemyAgent)
    {
        if (aIData.objectives == null)
        {
            return true;
        }
        
        return false;
    }
}
