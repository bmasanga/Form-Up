using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour, ILookable
{
    [SerializeField] private float turnSpeed = 200f;

    public void SetLookDirection(Vector2 input, bool isWorldPosition)
    {
        if (isWorldPosition)
            RotateTowardScreenPoint(input);
        else
            RotateTowardDirection(input);
    }

    private void RotateTowardDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void RotateTowardScreenPoint(Vector2 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
        Vector2 direction = (Vector2)worldPos - (Vector2)transform.position;
        RotateTowardDirection(direction);
    }
}