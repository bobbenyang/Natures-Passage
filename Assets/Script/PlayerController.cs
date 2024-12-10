using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float throwForce = 10f;
    public Transform holdPosition;
    public float pickupRadius = 2f;
    private Rigidbody heldObject;

    void Update()
    {
        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Picking up or throwing trash
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                ThrowTrash();
            }
        }
    }

    void TryPickup()
    {
        // Check for nearby objects within the pickup radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Trash"))
            {
                Rigidbody trashRigidbody = collider.GetComponent<Rigidbody>();
                if (trashRigidbody != null)
                {
                    PickupTrash(trashRigidbody);
                    return;
                }
            }
        }
    }

    void PickupTrash(Rigidbody trashRigidbody)
    {
        heldObject = trashRigidbody;
        heldObject.isKinematic = true; // Disable physics while holding
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.parent = holdPosition;
    }

    void ThrowTrash()
    {
        if (heldObject != null)
        {
            heldObject.isKinematic = false; // Re-enable physics
            heldObject.transform.parent = null;
            heldObject.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            heldObject = null;
        }
    }

    // Optional: Visualize the pickup radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}

