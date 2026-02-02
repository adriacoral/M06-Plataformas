using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0, 0, -10);
    
    [Header("Smoothing")]
    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f;
    
    [Header("Dead Zone (optional)")]
    public bool useDeadZone = false;
    public Vector2 deadZoneSize = new Vector2(1f, 0.5f);
    
    [Header("Bounds (optional)")]
    public bool useBounds = false;
    public Vector2 minBounds = new Vector2(-10, -5);
    public Vector2 maxBounds = new Vector2(10, 5);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        
        // Apply dead zone if enabled
        if (useDeadZone)
        {
            Vector3 currentPos = transform.position;
            
            // Only move if target is outside dead zone
            if (Mathf.Abs(target.position.x - currentPos.x) < deadZoneSize.x)
            {
                desiredPosition.x = currentPos.x;
            }
            if (Mathf.Abs(target.position.y - currentPos.y) < deadZoneSize.y)
            {
                desiredPosition.y = currentPos.y;
            }
        }
        
        // Smooth movement using SmoothDamp
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position, 
            desiredPosition, 
            ref velocity, 
            smoothSpeed
        );
        
        // Apply bounds if enabled
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }
        
        // Keep original Z position (important for 2D)
        smoothedPosition.z = offset.z;
        
        transform.position = smoothedPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (useDeadZone)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(deadZoneSize.x * 2, deadZoneSize.y * 2, 0));
        }
        
        if (useBounds)
        {
            Gizmos.color = Color.cyan;
            Vector3 center = new Vector3(
                (minBounds.x + maxBounds.x) / 2,
                (minBounds.y + maxBounds.y) / 2,
                0
            );
            Vector3 size = new Vector3(
                maxBounds.x - minBounds.x,
                maxBounds.y - minBounds.y,
                0
            );
            Gizmos.DrawWireCube(center, size);
        }
    }
}
