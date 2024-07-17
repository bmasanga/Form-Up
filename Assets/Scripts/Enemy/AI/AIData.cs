using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;

    public Transform currentTarget;
    public List<Transform> objective;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;

    public int GetObjectivesCount() => objective == null ? 0 : objective.Count;

    //private Objective _objective;

    void Start()
    {
        //_objective = FindObjectOfType<Objective>();
        //_objective.OnDestroyObjective.AddListener(RemoveObjective);

    }

    // void OnDestroy()
    // {
    //     if(_objective != null)
    //     {
    //         _objective.OnDestroyObjective.RemoveListener(RemoveObjective);

    //     }
    // }
    
    public Transform GetNextObjective()
    {
        for (int i = 0; i < GetObjectivesCount(); i++)
        {
            if (objective[i] != null)
            {
                return objective[i];
            }
        }
        return null;
    }

    public void RemoveObjective(Transform objectiveTransform)
    {
        for (int i = 0; i < objective.Count; i++)
        {
            if (objective[i] == objectiveTransform)
            {
                objective[i] = null;
                break;
            }
        }
    }
}
