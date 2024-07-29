using System.Collections;
using System.Linq;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private float aiUpdateDelay = 0.06f; 
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float attackDistance = 8f;

    private bool isChasing = false;

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
        isChasing = false;
    }

    public override void Update()
    {
        UpdateTarget();
        if (!isChasing && aIData.currentTarget != null)
        {
            isChasing = true;
            StartCoroutine(ChaseAndAttack());
        }
        OnMovementInput?.Invoke(movementInput);
    }

    private void UpdateTarget()
    {
        if (aIData.currentTarget == null && aIData.GetTargetsCount() > 0)
        {
            aIData.currentTarget = aIData.targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();
        }

        if (aIData.currentTarget != null)
        {
            OnPointerInput?.Invoke(aIData.currentTarget.position);
        }
    }

    private IEnumerator ChaseAndAttack()
    {
        while (aIData.currentTarget != null)
        {
            float distance = Vector2.Distance(aIData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                movementInput = Vector2.zero;
                OnMovementInput?.Invoke(movementInput);
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
            }
            else
            {
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aIData);
                OnMovementInput?.Invoke(movementInput);
                yield return new WaitForSeconds(aiUpdateDelay);
            }
        }

        movementInput = Vector2.zero;
        OnMovementInput?.Invoke(movementInput);
        isChasing = false;
    }
}
