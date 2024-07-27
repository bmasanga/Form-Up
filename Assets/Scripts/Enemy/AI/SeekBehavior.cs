using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float targetReachedThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    private Vector2 targetPositionCached;
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        // Cache the target position if it exists
        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
        {
            targetPositionCached = aiData.currentTarget.position;
        }

        // First check if we have reached the target
        if (Vector2.Distance(transform.position, targetPositionCached) < targetReachedThreshold)
        {
            aiData.currentTarget = null;
            return (danger, interest);
        }

        // If we haven't yet reached the target, do the main logic of finding the interest directions
        Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

            // Accept only directions at less than 90 degrees to the target direction
            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }

    // private void OnDrawGizmos()
    // {
    //     if (!showGizmo)
    //         return;

    //     Gizmos.DrawSphere(targetPositionCached, 0.2f);

    //     if (Application.isPlaying && interestsTemp != null)
    //     {
    //         Gizmos.color = Color.green;
    //         for (int i = 0; i < interestsTemp.Length; i++)
    //         {
    //             Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
    //         }
    //         if (aiData.currentTarget == null)
    //         {
    //             Gizmos.color = Color.red;
    //             Gizmos.DrawSphere(targetPositionCached, 0.1f);
    //         }
    //     }
    // }
}
