using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgentAnimations : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 200f;
    
    // private Animator animator;

    // private void Awake()
    // {
    //     animator = GetComponent<Animator>();
    // }

    public void RotateToPointer(Vector2 lookDirection)
    {
        // Vector3 scale = transform.localScale;
        // if (lookDirection.x > 0)
        // {
        //     scale.x = 1;
        // }
        // else if (lookDirection.x < 0)
        // {
        //     scale.x = -1;
        // }
        // transform.localScale = scale;

        // Vector3 mouseScreenPosition = new Vector3(lookInput.x, lookInput.y, Camera.main.nearClipPlane);
        // Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        // Vector3 direction = mouseWorldPosition - transform.position;
        //direction.z = 0f;

        if (lookDirection.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }

    // public void PlayAnimation(Vector2 movementInput)
    // {
    //     animator.SetBool("Running", movementInput.magnitude > 0);

    // }
}