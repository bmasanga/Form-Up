using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackState : State
{
    [SerializeField] GameObject indicator;
    [SerializeField] float attackDelay = 1f;
    private bool isAttacking = false;

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
        isAttacking = false;
        StopAllCoroutines();
    }

    public override void Update()
    {
        if (aIData != null)
        {
            OnPointerInput?.Invoke(aIData.currentTarget.position);

            float distance = Vector2.Distance(aIData.currentTarget.position, transform.position);
        }

        if (!isAttacking)
        {
            StartCoroutine(Attack());   
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        movementInput = Vector2.zero;
        OnAttackPressed?.Invoke();
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

}

