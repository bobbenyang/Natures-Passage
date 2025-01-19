using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCleaner : MonoBehaviour
{
    public float cleaningRange = 2f; // Range within which the player can clean trash
    public GameObject flowerPrefab; // Drag your flower prefab here in the Inspector
    public Animator animator; // Assign the Animator in the Inspector
    public float cleaningDelay = 2f; // Time it takes to clean the trash

    private bool isCleaning = false; // Prevent multiple cleaning actions at once

    void Update()
    {
        // Check if the player presses F and is not already cleaning
        if (Input.GetKeyDown(KeyCode.F) && !isCleaning)
        {
            // Start cleaning
            StartCoroutine(CleanNearbyTrash());
        }
    }

    private System.Collections.IEnumerator CleanNearbyTrash()
    {
        // Find all nearby objects with the "Trash" tag
        Collider[] hits = Physics.OverlapSphere(transform.position, cleaningRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Trash"))
            {
                isCleaning = true; // Prevent further cleaning actions

                // Play the cleaning animation
                animator.SetTrigger("Clean");

                // Wait for the delay duration
                yield return new WaitForSeconds(cleaningDelay);

                // Get the position of the trash
                Vector3 trashPosition = hit.transform.position;

                // Destroy the trash
                Destroy(hit.gameObject);

                // Spawn a flower at the same position
                Instantiate(flowerPrefab, trashPosition, Quaternion.identity);

                isCleaning = false; // Allow cleaning again
                break;
            }
        }
    }

    // To visualize the cleaning range in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, cleaningRange);
    }
}
