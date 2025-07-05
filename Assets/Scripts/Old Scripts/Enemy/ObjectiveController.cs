using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveController : MonoBehaviour
{
    static ObjectiveController instance;
    public UnityEvent<Transform> OnObjectiveDestroyed;
    
    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ObjectiveDestroyed(Transform objectiveTransform)
    {
        OnObjectiveDestroyed?.Invoke(objectiveTransform);
    }

}
