using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;

    public Transform currentTarget;
    public List<Transform> objectiveTransforms;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;

    public int GetObjectivesCount() => objectiveTransforms == null ? 0 : objectiveTransforms.Count;

    public Transform GetNextObjective()
    {
        if (GetObjectivesCount() > 0)
        {
            Debug.Log("trying to get objective");
            return objectiveTransforms[0];
        }
        return null;
    }

    // public void RemoveCurrentObjective()
    // {
    //     if (GetObjectivesCount() > 0)
    //     {
    //         objectiveTransforms.RemoveAt(0);
    //     }
    // }
}
