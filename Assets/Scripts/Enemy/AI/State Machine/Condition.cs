using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : ScriptableObject
{
   // might need to change AIData
   public virtual bool Test(AIData aIData)
   {
        return false;
   }
}
