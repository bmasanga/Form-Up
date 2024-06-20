using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
   public UnityEvent<Vector2> OnMoveInput, OnLookInput;
   public UnityEvent OnFireInput;
}
