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
        worldTarget = input;
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

    // // Added this method
    // public float ComputeLookAngleDeg(Vector2 input, bool isWorldPosition, Camera cam)
    // {
    //     if (!isWorldPosition)
    //     {
    //         if (input.sqrMagnitude < 0.0001f) return transform.eulerAngles.z;
    //         return Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90f;
    //     }

    //     if (cam == null) return transform.eulerAngles.z;

    //     Ray ray = cam.ScreenPointToRay(input);
    //     Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, transform.position.z));
    //     if (!plane.Raycast(ray, out float enter)) return transform.eulerAngles.z;
    //     Vector3 worldPoint = ray.GetPoint(enter);

    //     Vector2 dir = (Vector2)(worldPoint - transform.position);
    //     if (dir.sqrMagnitude < 0.0001f) return transform.eulerAngles.z;

    //     return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
    // }
    
    // public float GetLookAngleDeg() => transform.eulerAngles.z;
    
}