using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeCondition", menuName = "Conditions/TimeCondition")]
public class TimeCondition : Condition
{
    [SerializeField] float timeToWait = 2f;
    [SerializeField] float timePassed = 0;

    public override bool Test(AIData aIData, EnemyAgent enemyAgent)
    {
        timePassed += Time.deltaTime;
        if (timePassed >= timeToWait)
        {
            timePassed = 0;
            return true;
        }
        return false;
    }

}
