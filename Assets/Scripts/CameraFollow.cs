using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private Vector3 offset;   // Offset position relative to the player
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothing factor for the camera movement

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset; // Calculate the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Smooth the movement
            transform.position = smoothedPosition; // Update the camera position
        }
    }
}
