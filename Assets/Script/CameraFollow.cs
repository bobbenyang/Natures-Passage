using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the cube (player)
    public Vector3 offset = new Vector3(0, 5, -10); // Default camera offset
    public float smoothSpeed = 0.125f; // Smoothness factor for camera movement

    void LateUpdate()
    {
        // Keep the camera's height constant by ignoring the target's vertical position
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.y = transform.position.y; // Maintain the camera's current height

        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the smoothed position to the camera
        transform.position = smoothedPosition;

        // Optionally look at the target
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
    }
}