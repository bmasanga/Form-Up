using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveState : State
{
    [SerializeField] GameObject indicator;
    [SerializeField] float aiUpdateDelay = 0.06f;
    //[SerializeField] float objectiveReachedThreshold = 1f;

    bool navigating = false;

    public override void OnEnable()
    {
        base.OnEnable();
        if (indicator)
        {
            indicator.SetActive(true);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (indicator)
        {
            indicator.SetActive(false);
        }
        StopAllCoroutines();
        aIData.currentTarget = null;

    }

    public override void Update()
    {  
         //Enemy AI movement based on Target availability
        if (aIData.currentTarget != null)
        {
            //Looking at the Target
            OnPointerInput?.Invoke(aIData.currentTarget.position);
            if (navigating == false)
            {
                navigating = true;
                StartCoroutine(NavigateToObjective());
            }
        }
        else if (aIData.GetObjectivesCount() > 0)
        {
            //Target acquisition logic
            aIData.currentTarget = aIData.GetNextObjective();
        }
        //Moving the Agent
        OnMovementInput?.Invoke(movementInput);
    }

    private IEnumerator NavigateToObjective()
    {
        if (aIData.currentTarget == null)
        {
            //Stopping Logic
            //Debug.Log("Stopping");
            movementInput = Vector2.zero;
            navigating = false;
            yield break;
        }
        else
        {
            //Navigate logic
            movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aIData);
            yield return new WaitForSeconds(aiUpdateDelay);
            StartCoroutine(NavigateToObjective());
            
        }

    }
}
