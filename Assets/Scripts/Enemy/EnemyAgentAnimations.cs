using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgentAnimations : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 200f;

    public void RotateToPointer(Vector2 lookDirection)
    {

        if (lookDirection.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }
}