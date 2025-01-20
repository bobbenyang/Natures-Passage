using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public Transform pointA; // The starting point
    public Transform pointB; // The second point
    public Transform pointC; // The final point
    public float speed = 2f; // Movement speed
    public float rotationSpeed = 5f; // Rotation speed

    private Transform[] points; // Array of points to move between
    private int currentTargetIndex = 0; // The current target point index

    void Start()
    {
        // Store the points in an array for easy access
        points = new Transform[] { pointA, pointB, pointC };
    }

    void Update()
    {
        if (points.Length > 0)
        {
            MoveToTarget(points[currentTargetIndex]);
        }
    }

    void MoveToTarget(Transform target)
    {
        // Calculate the direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Rotate smoothly towards the target
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Check if the character has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentTargetIndex++;

            // Reset to the first point when the last point is reached
            if (currentTargetIndex >= points.Length)
            {
                currentTargetIndex = 0;
            }
        }
    }
}
