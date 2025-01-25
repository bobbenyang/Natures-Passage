using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashLifter : MonoBehaviour
{
    public Transform spawnArea; // Area where people spawn
    public GameObject personPrefab; // Prefab of the person to spawn
    public Transform destination; // Target destination for the person with trash
    public float moveSpeed = 3f; // Movement speed of the person
    public float pickupHeight = 2f; // Height above the head for lifted trash

    private GameObject liftedTrash; // Reference to the currently lifted trash
    private bool isCarryingTrash = false; // Whether the person is carrying trash

    void Start()
    {
        // Spawn the person at the spawn area
        SpawnPerson();
    }

    void Update()
    {
        // Move the person towards the destination if carrying trash
        if (isCarryingTrash && destination != null)
        {
            MoveTowards(destination.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "Trash"
        if (other.CompareTag("Trash") && !isCarryingTrash)
        {
            // Pick up the trash
            PickUpTrash(other.gameObject);
        }
    }

    private void SpawnPerson()
    {
        if (spawnArea != null && personPrefab != null)
        {
            // Spawn the person at the spawn area's position
            Instantiate(personPrefab, spawnArea.position, Quaternion.identity, spawnArea);
        }
    }

    private void PickUpTrash(GameObject trash)
    {
        isCarryingTrash = true;
        liftedTrash = trash;

        // Disable physics on the trash to prevent unintended movement
        Rigidbody trashRb = trash.GetComponent<Rigidbody>();
        if (trashRb != null)
        {
            trashRb.isKinematic = true;
        }

        // Set the trash position above the person's head
        trash.transform.SetParent(transform);
        trash.transform.localPosition = new Vector3(0, pickupHeight, 0);
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the person has reached the destination
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            DropTrash();
        }
    }

    private void DropTrash()
    {
        if (liftedTrash != null)
        {
            // Detach the trash and reset its physics
            liftedTrash.transform.SetParent(null);
            Rigidbody trashRb = liftedTrash.GetComponent<Rigidbody>();
            if (trashRb != null)
            {
                trashRb.isKinematic = false;
            }

            // Place the trash at the destination
            liftedTrash.transform.position = destination.position;
            liftedTrash = null;
        }

        isCarryingTrash = false;
    }
}