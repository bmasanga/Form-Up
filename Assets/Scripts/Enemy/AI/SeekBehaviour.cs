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
    private bool reachedLastTarget = true;


    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        if (aiData.currentTarget != null)
        {
            if (aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
            {
                targetPositionCached = aiData.currentTarget.position;
                reachedLastTarget = false;
                //Debug.Log("targetPositionCached: " + targetPositionCached);
            }
            else if (aiData.objectives.Contains(aiData.currentTarget))
            {
                targetPositionCached = aiData.currentTarget.position;
                reachedLastTarget = false;
                //Debug.Log("objectivePositionCached: " + targetPositionCached);

            }
        }


        if (Vector2.Distance(transform.position, targetPositionCached) < targetReachedThreshold)
        {
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, interest);
        }

        Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);
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

    private void OnDrawGizmos()
    {

        if (showGizmo == false)
            return;
        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            if (interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i]*2);
                }
                if (reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, 0.1f);
                }
            }
        }
    }
}
