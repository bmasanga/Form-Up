using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;

    public Transform currentTarget;
    public List<Transform> objectives;

    ObjectiveController objectiveController;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;

    public int GetObjectivesCount() => objectives == null ? 0 : objectives.Count;

    void OnEnable()
    {
        objectiveController = FindObjectOfType<ObjectiveController>();
        if (objectiveController != null)
        {
            objectiveController.OnObjectiveDestroyed.AddListener(RemoveObjective);
        }

    }

    void OnDestroy()
    {
        if (objectiveController != null)
        {
            objectiveController.OnObjectiveDestroyed.RemoveListener(RemoveObjective);
        }
    }

    public Transform GetNextTarget()
    {
        for (int i = 0; i <GetTargetsCount(); i++)
        {
            if (targets[i] != null)
            {
                return targets[i];
            }
        }
        return null;
    }
    
    
    public Transform GetNextObjective()
    {
        for (int i = 0; i < GetObjectivesCount(); i++)
        {
            if (objectives[i] != null)
            {
                return objectives[i];
            }
        }
        return null;
    }

    public void RemoveObjective(Transform objectiveTransform)
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i] == objectiveTransform)
            {
                objectives[i] = null;
                break;
            }
        }
    }
}
