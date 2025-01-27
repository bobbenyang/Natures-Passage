using UnityEngine;
using System.Collections.Generic;

public class DragTrash : MonoBehaviour
{
    private Camera mainCamera;
    private List<Rigidbody> selectedRigidbodies = new List<Rigidbody>();
    private Vector3 offset;
    private float zCoordinate;

    public Vector3 dragBoxSize = new Vector3(5f, 5f, 5f); // Size of the detection box
    public float dragSpeed = 10f; // Speed for dragging the objects
    public GameObject areaIndicatorPrefab; // Prefab for the draggable area indicator

    private GameObject areaIndicator; // Instance of the area indicator
    private bool isDragging = false; // Tracks whether the player is dragging

    void Start()
    {
        mainCamera = Camera.main;

        // Instantiate the area indicator
        if (areaIndicatorPrefab != null)
        {
            areaIndicator = Instantiate(areaIndicatorPrefab);
            areaIndicator.transform.localScale = dragBoxSize;
            areaIndicator.SetActive(false); // Hide it initially
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            TryPickUpObjects();
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            DropObjects();
        }

        if (isDragging && areaIndicator != null)
        {
            UpdateAreaIndicatorPosition();
        }
    }

    void FixedUpdate()
    {
        if (selectedRigidbodies.Count > 0)
        {
            DragObjects();
        }
    }

    void TryPickUpObjects()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Trash"))
            {
                // Get the position of the clicked object
                Vector3 center = hit.collider.transform.position;

                // Show the area indicator and start dragging
                if (areaIndicator != null)
                {
                    areaIndicator.SetActive(true);
                    isDragging = true;
                }

                // Find all trash objects within the box
                Collider[] colliders = Physics.OverlapBox(center, dragBoxSize / 2);

                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Trash"))
                    {
                        Rigidbody rb = collider.GetComponent<Rigidbody>();

                        if (rb != null && !selectedRigidbodies.Contains(rb))
                        {
                            selectedRigidbodies.Add(rb);

                            // Disable gravity for smoother dragging
                            rb.useGravity = false;
                            rb.freezeRotation = true;
                        }
                    }
                }

                // Set up offset for mouse dragging
                zCoordinate = mainCamera.WorldToScreenPoint(center).z;
                offset = center - GetMouseWorldPosition();
            }
        }
    }

    void DropObjects()
    {
        foreach (Rigidbody rb in selectedRigidbodies)
        {
            if (rb != null)
            {
                // Re-enable gravity
                rb.useGravity = true;
                rb.freezeRotation = false;
            }
        }

        selectedRigidbodies.Clear();

        // Hide the area indicator
        if (areaIndicator != null)
        {
            areaIndicator.SetActive(false);
        }

        isDragging = false;
    }

    void DragObjects()
    {
        Vector3 targetPosition = GetMouseWorldPosition() + offset;

        foreach (Rigidbody rb in selectedRigidbodies)
        {
            if (rb != null)
            {
                Vector3 force = (targetPosition - rb.transform.position) * dragSpeed;
                rb.velocity = force;
            }
        }
    }

    void UpdateAreaIndicatorPosition()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        areaIndicator.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, mouseWorldPosition.z);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zCoordinate;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
