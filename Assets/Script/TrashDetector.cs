using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashDetector : MonoBehaviour
{
    [Header("Detection Area")]
    public Vector3 center; // Center of the overlap box relative to the object
    public Vector3 halfExtents; // Half-size of the overlap box
    public LayerMask detectionLayer; // Layer for detecting trash objects

    [Header("UI Elements")]
    public Slider trashBar; // Slider to represent the trash count
    public int maxTrashCount = 10; // Maximum number of trash that can be detected

    void Update()
    {
        DetectTrash();
    }

    void DetectTrash()
    {
        // Perform the OverlapBox detection
        Collider[] detectedObjects = Physics.OverlapBox(transform.position + center, halfExtents, Quaternion.identity, detectionLayer);

        // Count objects with the "Trash" tag
        int trashCount = 0;
        foreach (Collider obj in detectedObjects)
        {
            if (obj.CompareTag("Trash"))
            {
                trashCount++;
            }
        }

        // Update the UI bar
        UpdateTrashBar(trashCount);
    }

    void UpdateTrashBar(int trashCount)
    {
        // Clamp the trash count to avoid exceeding the maximum
        trashCount = Mathf.Clamp(trashCount, 0, maxTrashCount);

        // Calculate the fill percentage and update the slider value
        float fillAmount = (float)trashCount / maxTrashCount;

        // Invert the fill amount to make it fill from right to left
        trashBar.value = 1.0f - fillAmount;
    }

    void OnDrawGizmos()
    {
        // Draw the OverlapBox in the Scene view for visualization
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + center, halfExtents * 2);
    }
}