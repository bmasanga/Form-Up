using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour, ILookable
{
    [SerializeField] private float turnSpeed           = 200f;
    [SerializeField] private float minInputMagnitude   = 0.1f;

    private bool    hasWorldTarget = false;
    private Vector2 worldTarget    = Vector2.zero;
    private Camera  playerCamera;

    private void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
            Debug.LogError("LookController: no camera tagged MainCamera in scene.");
    }

    public void SetLookDirection(Vector2 input, bool isWorldPosition)
    {
        if (!isWorldPosition)
        {
            // stick-based rotation (unchanged)
            RotateTowardStick(input);
            hasWorldTarget = false;
            return;
        }

        // mouse-based rotation
        worldTarget    = input;
        hasWorldTarget = true;
    }

    private void Update()
    {
        if (!hasWorldTarget || playerCamera == null) 
            return;

        RotateTowardMouse(worldTarget);
    }

    private void RotateTowardMouse(Vector2 screenPosition)
    {
        // 1) Ray from camera through mouse cursor
        Ray ray = playerCamera.ScreenPointToRay(screenPosition);

        // 2) Plane at Z = this.transform.position.z
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, transform.position.z));

        // 3) Find intersection
        if (!plane.Raycast(ray, out float enter))
            return;
        Vector3 worldPoint = ray.GetPoint(enter);

        // 4) Compute direction & rotate
        Vector2 direction = (Vector2)(worldPoint - transform.position);
        if (direction.sqrMagnitude < minInputMagnitude * minInputMagnitude)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            turnSpeed * Time.deltaTime
        );
    }

    private void RotateTowardStick(Vector2 stickInput)
    {
        if (stickInput.sqrMagnitude < minInputMagnitude * minInputMagnitude)
            return;
        float angle = Mathf.Atan2(stickInput.y, stickInput.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            turnSpeed * Time.deltaTime
        );
    }
}