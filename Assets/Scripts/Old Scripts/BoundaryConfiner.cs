using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryConfiner : MonoBehaviour
{
    [SerializeField] CircleCollider2D boundaryCollider;

    void Update()
    {
        ConstrainPosition();    
    }

    void ConstrainPosition()
    {
        if (boundaryCollider != null)
        {
            // Bounds bounds = boundaryCollider.bounds;
            // Vector3 position = transform.position;
            // position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
            // position.y = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);
            // transform.position = position;

            Vector2 center = boundaryCollider.bounds.center;
            float radius = boundaryCollider.radius;
            Vector2 distanceToCenter = transform.position - (Vector3)center;
            if(distanceToCenter.magnitude > radius)
            {
                Vector2 clampedPosition = center + distanceToCenter.normalized * radius;
                transform.position = clampedPosition;
            }
        }
    }
}
