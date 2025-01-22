using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrashCounter : MonoBehaviour
{
    [Header("Detection Settings")]
    public Vector3 detectionCenter; // Center of the detection area (local to this object)
    public Vector3 detectionSize = new Vector3(5f, 5f, 5f); // Size of the detection area (width, height, depth)

    [Header("UI Components")]
    public TextMeshProUGUI trashCountText; // Assign your TextMeshPro UI component here

    private int trashCount = 0;

    private void Update()
    {
        CountTrashInArea();
        UpdateTrashCountUI();
    }

    private void CountTrashInArea()
    {
        // World position of the detection center
        Vector3 worldCenter = transform.position + detectionCenter;

        // Get all colliders in the detection box
        Collider[] detectedObjects = Physics.OverlapBox(worldCenter, detectionSize / 2, Quaternion.identity);

        // Count objects with the tag "Trash"
        trashCount = 0;
        foreach (var obj in detectedObjects)
        {
            if (obj.CompareTag("Trash"))
            {
                trashCount++;
            }
        }
    }

    private void UpdateTrashCountUI()
    {
        if (trashCountText != null)
        {
            trashCountText.text = "Trash in Area: " + trashCount;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection box in the Scene view
        Gizmos.color = Color.yellow;
        Vector3 worldCenter = transform.position + detectionCenter;
        Gizmos.DrawWireCube(worldCenter, detectionSize);
    }
}
