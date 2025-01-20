using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantUpwardForce : MonoBehaviour
{
    public float jumpForce = 10f; // Strength of the upward force
    public float jumpInterval = 2f; // Time in seconds between jumps

    private Rigidbody rb;
    private float timeSinceLastJump = 0f;
    private bool isCollidingWithTrash = false;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Check if the Rigidbody exists and is assigned
        if (rb == null)
        {
            rb = GetComponentInChildren<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("No Rigidbody attached to the object. Please add one.");
            }
        }
    }

    void Update()
    {
        // Increment the timer
        timeSinceLastJump += Time.deltaTime;

        // Apply the upward force if the object is colliding with "Trash" and the interval has elapsed
        if (isCollidingWithTrash && timeSinceLastJump >= jumpInterval && rb != null)
        {
            PerformJump();
            timeSinceLastJump = 0f; // Reset the timer
        }
    }

    void PerformJump()
    {
        // Apply a sudden upward force
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Trash"
        if (collision.gameObject.CompareTag("Trash"))
        {
            isCollidingWithTrash = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the object leaving the collision is tagged "Trash"
        if (collision.gameObject.CompareTag("Trash"))
        {
            isCollidingWithTrash = false;
        }
    }
}
