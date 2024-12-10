using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public float pickupRadius = 5f; // Radius to detect objects
    public LayerMask pickupLayer; // Layer to specify pickup objects

    void Update()
    {
        // Check for input to pick up objects
        if (Input.GetKeyDown(KeyCode.F))
        {
            PickupObjects();
        }
    }

    void PickupObjects()
    {
        // Find all colliders within the pickup radius on the specified layer
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, pickupRadius, pickupLayer);

        foreach (Collider obj in objectsInRange)
        {
            // Optionally add a check for a specific tag or component
            if (obj.CompareTag("Pickup"))
            {
                // Perform pickup action (e.g., deactivate or destroy the object)
                Debug.Log("Picked up: " + obj.name);
                Destroy(obj.gameObject); // Remove the object
            }
        }
    }

    // To visualize the pickup radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
