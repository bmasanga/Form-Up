using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackState : State
{
    [SerializeField] GameObject indicator;
    [SerializeField] float attackDelay = 1f;

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
    }

    public override void Update()
    {
        OnPointerInput?.Invoke(aIData.currentTarget.position);

        float distance = Vector2.Distance(aIData.currentTarget.position, transform.position);

        StartCoroutine(Attack());   
    }

    private IEnumerator Attack()
    {
        
        movementInput = Vector2.zero;
        OnAttackPressed?.Invoke();
        yield return new WaitForSeconds(attackDelay);
    }

}

