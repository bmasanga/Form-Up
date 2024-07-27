using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class ChaseState : State
{
    [SerializeField] GameObject indicator;
    [SerializeField] float aiUpdateDelay = 0.06f; 
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackDistance = 8f;

    bool following = false;

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
    }

    public override void Update()
    {    
         //Enemy AI movement based on Target availability
        // if (aIData.currentTarget != null)
        // {
        //     //Looking at the Target
        //     OnPointerInput?.Invoke(aIData.currentTarget.position);
        //     if (following == false)
        //     {
        //         following = true;
        //         StartCoroutine(ChaseAndAttack());
        //     }
        // }
        // else if (aIData.GetTargetsCount() > 0)
        // {
        //     //Target acquisition logic
        //     aIData.currentTarget = aIData.targets[0];
        // }
        // //Moving the Agent
        // OnMovementInput?.Invoke(movementInput);

        // Set the current target to the closest player target if available
        if (aIData.targets != null && aIData.targets.Count > 0)
        {
            aIData.currentTarget = aIData.targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();
        }
        // else
        // {
        //     // Set the current target to the next objective if no player targets are detected
        //     aIData.currentTarget = null;
        // }

        // Enemy AI movement based on target availability
        if (aIData.currentTarget != null)
        {
            // Looking at the target
            OnPointerInput?.Invoke(aIData.currentTarget.position);
            if (!following)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }

        // Moving the agent
        OnMovementInput?.Invoke(movementInput);

    }

    private IEnumerator ChaseAndAttack()
    {
        if (aIData.currentTarget == null)
        {
            //Stopping Logic
            //Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aIData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                //Attack logic
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                //Chase logic
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aIData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }

        }

    }

}

