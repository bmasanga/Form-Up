using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceivable
{
    void SetMoveInput(Vector2 input);
    void SetLookInput(Vector2 input, bool isWorldPosition);
    void SetFire(bool isPressed);
    void SetAction(bool isPressed);
    void SetToggle(bool isPressed);
    void SetTarget(bool isPressed);
}