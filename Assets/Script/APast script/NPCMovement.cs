using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float idleTime = 2f; // Time to pause between movements
    public Vector3 moveAreaCenter; // Center of the NPC's roaming area
    public Vector3 moveAreaSize;   // Size of the roaming area

    public GameObject objectToSpawn; // Object to spawn
    public float distanceThreshold = 5f; // Base distance before spawning an object
    public float extendedDistanceThreshold = 10f; // Extended distance threshold when there's enough trash
    public int trashCountForExtendedDistance = 5; // Number of trash objects required to extend distance

    private Vector3 targetPosition;
    private Vector3 lastSpawnPosition; // The position where the last object was spawned
    private bool isInteracting = false;

    private Animator animator; // Reference to the Animator

    void Start()
    {
        SetRandomTargetPosition();
        lastSpawnPosition = transform.position; // Initialize spawn position

        animator = GetComponent<Animator>(); // Get the Animator component
        if (animator == null)
        {
            Debug.LogWarning($"Animator not found on {gameObject.name}. Please add one.");
        }
    }

    void Update()
    {
        if (!isInteracting)
        {
            if (CheckTrashCount() > 0)
            {
                MoveToTarget();
                CheckSpawnCondition();
            }
            else
            {
                PauseMovement();
            }
        }
    }

    void SetRandomTargetPosition()
    {
        float x = Random.Range(moveAreaCenter.x - moveAreaSize.x / 2, moveAreaCenter.x + moveAreaSize.x / 2);
        float z = Random.Range(moveAreaCenter.z - moveAreaSize.z / 2, moveAreaCenter.z + moveAreaSize.z / 2);
        targetPosition = new Vector3(x, transform.position.y, z);
    }

    void MoveToTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Invoke(nameof(SetRandomTargetPosition), idleTime); // Wait before choosing a new position
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            Vector3 direction = (targetPosition - transform.position).normalized;

            if (direction != Vector3.zero) // Avoid NaN rotations
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed); // Smoothly rotate
            }
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
    }

    void CheckSpawnCondition()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastSpawnPosition);

        // Determine the current distance threshold based on trash count
        float currentDistanceThreshold = CheckTrashCount() >= trashCountForExtendedDistance ? extendedDistanceThreshold : distanceThreshold;

        if (distanceMoved >= currentDistanceThreshold)
        {
            SpawnObject();
            lastSpawnPosition = transform.position; // Update the last spawn position
        }
    }

    void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }
    }

    public void PauseMovement()
    {
        isInteracting = true;

        // Trigger the shock animation
        if (animator != null)
        {
            animator.SetBool("isWalking", false); // Stop walking animation
            animator.SetTrigger("ShockTrigger"); // Play shock animation
        }
    }

    public void ResumeMovement()
    {
        isInteracting = false;

        // Reset to walking animation
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        SetRandomTargetPosition();
    }

    public void LookAtPlayer(Vector3 playerPosition)
    {
        Vector3 direction = (playerPosition - transform.position).normalized;
        direction.y = 0; // Keep the NPC upright
        transform.rotation = Quaternion.LookRotation(direction);
    }

    int CheckTrashCount()
    {
        Collider[] colliders = Physics.OverlapBox(moveAreaCenter, moveAreaSize / 2);
        int trashCount = 0;

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.CompareTag("Trash"))
            {
                trashCount++;
            }
        }

        return trashCount;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(moveAreaCenter, moveAreaSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(moveAreaCenter, moveAreaSize);
    }
}
