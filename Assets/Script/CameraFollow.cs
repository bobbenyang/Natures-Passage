using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the cube (player)
    public Vector3 offset = new Vector3(0, 5, -10); // Default camera offset
    public float smoothSpeed = 0.1f; // Smoothness factor for camera movement
    public float rotationSpeed = 5f; // Speed to rotate towards the player

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate desired position with offset
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly move towards the target position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Gradually rotate the camera to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}