using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    public UnityEvent<Transform> OnDestroyObjective;

    private void OnDestroy()
    {
        OnDestroyObjective?.Invoke(transform);
    }
}
