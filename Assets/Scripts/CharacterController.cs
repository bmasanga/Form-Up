using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour, IInputReceivable
{
    private IMoveable moveController;
    private ILookable lookController;
    private PlayerInputController inputController;

    private void Awake()
    {
        moveController = GetComponent<IMoveable>();
        lookController = GetComponent<ILookable>();
        inputController = GetComponent<PlayerInputController>();

        inputController.Initialize(this);
    }

    public void SetMoveInput(Vector2 input)
    {
        moveController?.Move(input);
    }

    public void SetLookInput(Vector2 input)
    {
        lookController?.SetLookDirection(input, isWorldPosition: false); // fallback
    }

    public void SetLookInput(Vector2 input, bool isWorldPosition)
    {
        lookController?.SetLookDirection(input, isWorldPosition);
    }

    public void SetFire(bool isPressed)
    {
        // Hook to weapon system
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
}