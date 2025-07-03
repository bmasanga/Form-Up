using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OutOfRangeCondition", menuName = "Conditions/OutOfRangeCondition")]
public class OutOfRangeCondition : Condition
{
    [SerializeField] float attackRange = 15f;
    
    public override bool Test(AIData aIData, EnemyAgent enemyAgent)
    {

        if (aIData.currentTarget != null)
        {
            float distance = Vector2.Distance(aIData.currentTarget.position, enemyAgent.transform.position);
            return distance > attackRange;
        }
        return false;
    }
}
