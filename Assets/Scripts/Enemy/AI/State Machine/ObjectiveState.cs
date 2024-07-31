using System.Collections;
using UnityEngine;

public class ObjectiveState : State
{
    [SerializeField] GameObject indicator;
    //[SerializeField] float aiUpdateDelay = 0.06f;

    //bool navigating = false;

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
        // Update movement input every frame
        UpdateMovementInput();

        // Enemy AI movement based on Target availability
        if (aIData.currentTarget != null)
        {
            // Looking at the Target
            OnPointerInput?.Invoke(aIData.currentTarget.position);
        }
        else if (aIData.GetObjectivesCount() > 0)
        {
            // Target acquisition logic
            aIData.currentTarget = aIData.GetNextObjective();
            Debug.Log("Current Target Objective Position: " + aIData.currentTarget.position);
        }

    }

    private void UpdateMovementInput()
    {
        if (aIData.currentTarget != null)
        {
            movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aIData);
        }
        else
        {
            movementInput = Vector2.zero;
        }
        OnMovementInput?.Invoke(movementInput);
    }
}
