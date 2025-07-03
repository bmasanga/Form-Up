using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    ObjectiveController objectiveController;

    void Awake()
    {
        objectiveController = FindObjectOfType<ObjectiveController>();
    }
    
    private void OnDestroy()
    {
        if (objectiveController != null)  
        {  
        objectiveController.ObjectiveDestroyed(transform);
        }
    }
}
