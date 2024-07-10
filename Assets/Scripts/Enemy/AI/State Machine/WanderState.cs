using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderState : State
{
    // [SerializeField] float angleModifier = 1;
    [SerializeField] bool isWandering = false;
    [SerializeField] float wanderingSpeed = 0.5f;
    //[SerializeField] float wanderTime = 2.0f;
    [SerializeField] float waitTime = 2.0f;

    [SerializeField] float randomMovementRange = 5.0f;

    // Vector2? direction = null;

    Vector3 targetPosition;
    Vector2? direction;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Update()
    {
        if (isWandering)
        {
            if (direction.HasValue)
            {
                
                OnPointerInput?.Invoke(targetPosition);
                OnMovementInput?.Invoke(movementInput);

                CheckIfReachedTarget();
                
            }
            return;
        }
        isWandering = true;
        WanderAround();
    }

    void WanderAround()
    {
        targetPosition = GetRandomPointInCircle();

        direction = (targetPosition - enemyAgent.transform.position).normalized;
        movementInput = direction.Value.normalized * wanderingSpeed;
        
        Debug.Log("targetPosition: " + targetPosition);
        Debug.Log("direction: " + direction.Value);
        Debug.Log("movementInput: " + movementInput);

        //yield return new WaitForSecondsRealtime(wanderTime);

        //StartCoroutine(StopAndWait());
    }

    Vector3 GetRandomPointInCircle()
    {
        return enemyAgent.transform.position + (Vector3)Random.insideUnitCircle * randomMovementRange;
    }

    void CheckIfReachedTarget()
    {
        if ((enemyAgent.transform.position - targetPosition).sqrMagnitude < 1.0f)
        {
            movementInput = Vector2.zero;
            direction = null;
            OnMovementInput?.Invoke(movementInput);
            StartCoroutine(StopAndWait());
        }
    }

    IEnumerator StopAndWait()
    {
        // movementInput = Vector2.zero;
        // direction = null;
        // OnMovementInput?.Invoke(movementInput);

        yield return new WaitForSecondsRealtime(waitTime);
        isWandering = false;
    }
}
