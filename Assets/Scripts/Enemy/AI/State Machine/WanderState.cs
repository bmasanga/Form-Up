using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderState : State
{
    // [SerializeField] float angleModifier = 1;
    [SerializeField] bool isWandering = false;
    [SerializeField] float wanderingSpeed = 0.5f;
    [SerializeField] float wanderTime = 2.0f;
    [SerializeField] float waitTime = 2.0f;

    [SerializeField] float randomMovementRange = 5.0f;

    // Vector2? direction = null;

    Vector3 targetPosition;
    Vector2? direction;

    public override void OnEnable()
    {
        base.OnEnable();
        //targetPosition = GetRandomPointInCircle();
    }

    // public override void Update()
    // {
    //     base.Update();

    //     direction = (targetPosition - enemyAgent.transform.position).normalized;
    //     Vector2 movementInput = direction * wanderingSpeed;
    //     OnMovementInput?.Invoke(movementInput);
    //     OnPointerInput?.Invoke(direction);

    //     if ((enemyAgent.transform.position - targetPosition).sqrMagnitude < 1f)
    //     {
    //         StartCoroutine(WaitAndChangeTarget());
    //     }
    // }

    // IEnumerator WaitAndChangeTarget()
    // {
    //     movementInput = Vector2.zero;
    //     OnMovementInput?.Invoke(movementInput);

    //     yield return new WaitForSecondsRealtime wanderTime);

    //     targetPosition = GetRandomPointInCircle();
    // }

    public override void Update()
    {
        if (isWandering)
        {
            if (direction.HasValue)
            {
                
                OnPointerInput?.Invoke(direction.Value);
                OnMovementInput?.Invoke(movementInput);
                
            }
            return;
        }
        isWandering = true;
        StartCoroutine(WanderAround());
    }

    IEnumerator WanderAround()
    {
        targetPosition = GetRandomPointInCircle();

        direction = (targetPosition - enemyAgent.transform.position).normalized;
        movementInput = direction.Value.normalized * wanderingSpeed;
        
        Debug.Log("direction: " + direction.Value);
        Debug.Log("movementInput: " + movementInput);

        yield return new WaitForSecondsRealtime(wanderTime);

        StartCoroutine(StopAndWait());
    }

    Vector3 GetRandomPointInCircle()
    {
        return enemyAgent.transform.position + (Vector3)Random.insideUnitCircle * randomMovementRange;
    }

    IEnumerator StopAndWait()
    {
        movementInput = Vector2.zero;
        direction = null;
        OnMovementInput?.Invoke(movementInput);

        yield return new WaitForSecondsRealtime(waitTime);
        isWandering = false;
    }
}
