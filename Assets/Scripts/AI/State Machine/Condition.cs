using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : ScriptableObject
{
   // might need to change EnemyAgent to EnemyAI
   public virtual bool Test(EnemyAgent agent)
   {
        return false;
   }
}
