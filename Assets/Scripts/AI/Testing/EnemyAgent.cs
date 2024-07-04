using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyAgent : MonoBehaviour
{
    EnemyAgentAnimations agentAnimations;
    EnemyAgentMover agentMover;
    EnemyCannon enemyCannon;

    

    //private WeaponParent weaponParent;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

     private void Awake()
    {
        agentAnimations = GetComponent<EnemyAgentAnimations>();
        // weaponParent = GetComponentInChildren<WeaponParent>();
        agentMover = GetComponent<EnemyAgentMover>();
        enemyCannon = GetComponentInChildren<EnemyCannon>();
    }
    
    private void Update()
    {
        // pointerInput = GetPointerInput();
        // movementInput = movement.action.ReadValue<Vector2>().normalized;

        agentMover.MovementInput = MovementInput;
        AnimateCharacter();
    }

    public void PerformAttack()
    {
        enemyCannon.Fire();
    }


    private void AnimateCharacter()
    {
        Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        agentAnimations.RotateToPointer(lookDirection);
        //agentAnimations.PlayAnimation(MovementInput);
    }


}