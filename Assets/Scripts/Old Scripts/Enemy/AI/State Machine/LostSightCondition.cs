using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LostSightCondition", menuName = "Conditions/LostSightCondition")]
public class LostSightCondition : Condition
{
    [SerializeField] float timeToWait = 5f;
    [SerializeField] float timePassed = 0;
    public override bool Test(AIData aIData, EnemyAgent enemyAgent)
    {
        if(aIData.currentTarget == null)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= timeToWait)
            {
                timePassed = 0;
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            timePassed = 0;
            return false;
        }
    }
}
