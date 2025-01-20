using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationAngle = 45f; // Maximum angle to rotate in each direction
    public float rotationSpeed = 2f;  // Speed of rotation

    private float targetAngle;        // The angle the enemy is rotating toward
    private bool rotatingRight = true; // Determines rotation direction

    void Start()
    {
        // Set initial target angle
        targetAngle = rotationAngle;
    }

    void Update()
    {
        RotateBackAndForth();
    }

    void RotateBackAndForth()
    {
        // Calculate the current rotation angle
        float currentYRotation = transform.rotation.eulerAngles.y;

        // Smoothly rotate toward the target angle
        float step = rotationSpeed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

        // Check if the enemy has reached the target rotation angle
        if (Mathf.Abs(Mathf.DeltaAngle(currentYRotation, targetAngle)) < 1f)
        {
            FlipDirection();
        }
    }

    void FlipDirection()
    {
        // Switch the direction of rotation
        rotatingRight = !rotatingRight;

        // Set the new target angle
        targetAngle = rotatingRight ? rotationAngle : -rotationAngle;
    }
}
