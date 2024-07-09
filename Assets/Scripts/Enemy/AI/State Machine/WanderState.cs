using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderState : State
{
    // [SerializeField] float angleModifier = 1;
    //[SerializeField] bool isWandering = false;
    [SerializeField] float wanderingSpeed = 0.5f;
    //[SerializeField] float waitDelay = 2.0f;

    [SerializeField] float randomMovementRange = 5.0f;

    // Vector2? direction = null;

    Vector3 targetPosition;
    Vector3 direction;

    public override void OnEnable()
    {
        base.OnEnable();
        targetPosition = GetRandomPointInCircle();
    }

    public override void Update()
    {
        base.Update();
        // if (isWandering)
        // {
        //     if (direction.HasValue)
        //     {
        //         movementInput = direction.Value.normalized * wanderingSpeed;
        //         OnMovementInput?.Invoke(movementInput);
        //     }
        //     return;
        // }
        // isWandering = true;
        // StartCoroutine(WanderAround());

        direction = (targetPosition - enemyAgent.transform.position).normalized;
        movementInput = direction * wanderingSpeed;
        OnMovementInput?.Invoke(movementInput);

        if ((enemyAgent.transform.position - targetPosition).sqrMagnitude < 0.01f)
        {
            targetPosition = GetRandomPointInCircle();
        }
    }



    // IEnumerator WanderAround()
    // {
    
    //     float wanderOrientation = Random.Range(-30f, 30f) * angleModifier;
    //     var newRotation = Quaternion.AngleAxis(wanderOrientation, Vector3.up);
    //     var rotationDirection = newRotation * Vector3.forward;
    //     direction = rotationDirection;
    //     yield return new WaitForSecondsRealtime(waitDelay);
    // }

    Vector3 GetRandomPointInCircle()
    {
        return enemyAgent.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * randomMovementRange;
    }
}
