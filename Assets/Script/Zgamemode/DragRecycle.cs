using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRecycle : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector3 offset;
    private float zCoordinate;
    private Rigidbody selectedRigidbody;
    private bool isDragging;

    public float dragSpeed = 10f; // Speed to move the object

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            TryPickUpObject();
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            DropObject();
        }
    }

    void FixedUpdate()
    {
        if (isDragging && selectedObject != null)
        {
            DragObject();
        }
    }

    void TryPickUpObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object has either "Trash" or "Recycle" tag
            if (hit.collider.CompareTag("Trash") || hit.collider.CompareTag("Recycle"))
            {
                selectedObject = hit.collider.gameObject;
                selectedRigidbody = selectedObject.GetComponent<Rigidbody>();

                if (selectedRigidbody != null)
                {
                    // Prepare for dragging
                    selectedRigidbody.useGravity = false;
                    selectedRigidbody.freezeRotation = true;

                    zCoordinate = mainCamera.WorldToScreenPoint(selectedObject.transform.position).z;
                    offset = selectedObject.transform.position - GetMouseWorldPosition();

                    isDragging = true;
                }
            }
        }
    }

    void DropObject()
    {
        if (selectedObject != null && selectedRigidbody != null)
        {
            // Re-enable Rigidbody physics
            selectedRigidbody.useGravity = true;
            selectedRigidbody.freezeRotation = false;

            selectedRigidbody = null;
            selectedObject = null;
            isDragging = false;
        }
    }

    void DragObject()
    {
        // Calculate the target position based on mouse position
        Vector3 targetPosition = GetMouseWorldPosition() + offset;

        // Move the object using Rigidbody to avoid clipping
        Vector3 force = (targetPosition - selectedObject.transform.position) * dragSpeed;
        selectedRigidbody.velocity = force;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zCoordinate;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}