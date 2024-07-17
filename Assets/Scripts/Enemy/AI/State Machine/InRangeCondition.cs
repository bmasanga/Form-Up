using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InRangeCondition", menuName = "Conditions/InRangeCondition")]
public class InRangeCondition : Condition
{
    [SerializeField] float attackRange = 15f;
    
    public override bool Test(AIData aIData, EnemyAgent enemyAgent)
    {

        if (aIData.currentTarget != null)
        {
            float distance = Vector2.Distance(aIData.currentTarget.position, enemyAgent.transform.position);
            return distance < attackRange;
        }
        return false;
    }
}
