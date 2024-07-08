using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("Stopping");
        movementInput = Vector2.zero;
    }
}
