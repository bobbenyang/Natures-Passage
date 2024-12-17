using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform pointA; // Start point
    public Transform pointB; // End point

    [Header("Movement Settings")]
    public float speed = 2f; // Movement speed
    public float rotationSpeed = 2f; // Speed of rotation

    private Vector3 targetPosition; // Current target position
    private bool movingToB = true; // Determines movement direction
    private bool isRotating = false; // Flag to control rotation state

    void Start()
    {
        // Start at pointA
        transform.position = pointA.position;
        targetPosition = pointB.position;
    }

    void Update()
    {
        if (!isRotating)
        {
            MoveBetweenPoints();
        }
    }

    void MoveBetweenPoints()
    {
        // Move the enemy toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the enemy reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(Rotate180());
        }
    }

    System.Collections.IEnumerator Rotate180()
    {
        isRotating = true;

        // Determine the target rotation (180 degrees around Y-axis)
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 180, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure exact rotation
        FlipDirection();

        isRotating = false;
    }

    void FlipDirection()
    {
        // Swap movement direction
        movingToB = !movingToB;

        // Set the new target position
        targetPosition = movingToB ? pointB.position : pointA.position;
    }
}
