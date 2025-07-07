using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour, ILookable
{
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private float minInputMagnitude = 0.1f;

    private bool hasWorldTarget = false;
    private Vector2 worldTarget = Vector2.zero;
    private Camera playerCamera;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    public void SetLookDirection(Vector2 input, bool isWorldPosition)
    {
        
        if (!isWorldPosition)
        {
            RotateTowardStick(input);
            return;
        }

        hasWorldTarget = true;
        worldTarget = input;
    }

    private void Update()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
                return;
        }

        if (hasWorldTarget)
        {
            RotateTowardMouse(worldTarget);
        }
    }

    private void RotateTowardMouse(Vector2 screenPosition)
    {
        Vector3 mouseScreenPosition = new Vector3(screenPosition.x, screenPosition.y, playerCamera.nearClipPlane);
        Vector3 mouseWorldPosition = playerCamera.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0f;

        if (direction.magnitude < minInputMagnitude)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
     private void RotateTowardStick(Vector2 stickInput)
    {
        if (stickInput.magnitude < minInputMagnitude)
            return;

        float angle = Mathf.Atan2(stickInput.y, stickInput.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}