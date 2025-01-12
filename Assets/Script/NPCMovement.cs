using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float idleTime = 2f; // Time to pause between movements
    public Vector3 moveAreaCenter; // Center of the NPC's roaming area
    public Vector3 moveAreaSize;   // Size of the roaming area

    private Vector3 targetPosition;
    private bool isInteracting = false;

    void Start()
    {
        SetRandomTargetPosition();
    }

    void Update()
    {
        if (!isInteracting)
        {
            MoveToTarget();
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
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
        }
    }

    public void PauseMovement()
    {
        isInteracting = true;
    }

    public void ResumeMovement()
    {
        isInteracting = false;
        SetRandomTargetPosition();
    }

    public void LookAtPlayer(Vector3 playerPosition)
    {
        Vector3 direction = (playerPosition - transform.position).normalized;
        direction.y = 0; // Keep the NPC upright
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
