using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILookable
{
    /// <param name="input">Vector2 stick direction or screen position</param>
    /// <param name="isWorldPosition">True if input is screen/world position (e.g., mouse)</param>
    void SetLookDirection(Vector2 input, bool isWorldPosition);
}