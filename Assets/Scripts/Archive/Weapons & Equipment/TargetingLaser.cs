using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TargetingLaser : MonoBehaviour
{
    [SerializeField] float laserLength = 10f; // Maximum length of the laser
    [SerializeField] LayerMask layerMask; // Layers to hit

    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2; // Start and end points of the laser
        }
        else
        {
            Debug.LogError("LineRenderer component not found on object: " + gameObject.name);
        }
    }

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        Vector2 laserStart = transform.position;
        Vector2 laserEnd = laserStart + (Vector2)(transform.up * laserLength);

        RaycastHit2D hit = Physics2D.Raycast(laserStart, transform.up, laserLength, layerMask);

        if (hit.collider != null)
        {
            // If the laser hits something, set the end point to the hit point
            laserEnd = hit.point;
        }

        DrawLaser(laserStart, laserEnd);
    }

    void DrawLaser(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void SetLaserColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void SetLaserEnabled(bool enabled)
    {
        lineRenderer.enabled = enabled;
    }

}
