using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour, IInputReceivable
{
    private IMoveable moveController;
    private ILookable lookController;
    private IWeapon weapon;
    private PlayerInputController inputController;

    private bool isFiring = false;

    private void Awake()
    {
        moveController = GetComponent<IMoveable>();
        lookController = GetComponent<ILookable>();
        inputController = GetComponent<PlayerInputController>();
        weapon = GetComponentInChildren<IWeapon>();

        inputController.Initialize(this);
    }


    public void SetMoveInput(Vector2 input)
    {
        moveController?.Move(input);
    }

    public void SetLookInput(Vector2 input, bool isWorldPosition)
    {
        lookController?.SetLookDirection(input, isWorldPosition);
    }

    public void SetFire(bool isPressed)
    {
        isFiring = isPressed;
    }

    public void SetAction(bool isPressed)
    {
        // Hook to action logic
    }

    public void SetToggle(bool isPressed)
    {
        // Hook to movement mode or stance
    }

    public void SetTarget(bool isPressed)
    {
        // Hook to targeting system
    }

    private void FixedUpdate()
    {
        if (isFiring)
        {
            weapon?.Fire();
        }
    }
}