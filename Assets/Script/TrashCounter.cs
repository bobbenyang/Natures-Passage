using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCounterBar : MonoBehaviour
{
    [Header("Detection Settings")]
    public Vector3 detectionCenter; // Center of the detection area (local to this object)
    public Vector3 detectionSize = new Vector3(5f, 5f, 5f); // Size of the detection area (width, height, depth)

    [Header("UI Components")]
    public Image trashBar; // Assign the UI bar here (Image component)
    
    [Header("Bar Settings")]
    public int maxTrash = 100; // Maximum trash amount the bar represents
    public Gradient barColor; // Gradient for bar color (green to red)

    private int trashCount = 0;

    private void Update()
    {
        CountTrashInArea();
        UpdateTrashBar();
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

    private void UpdateTrashBar()
    {
        if (trashBar != null)
        {
            // Clamp the trash count to the maximum allowed value
            float fillAmount = Mathf.Clamp01((float)trashCount / maxTrash);

            // Update the bar fill amount
            trashBar.fillAmount = fillAmount;

            // Update the bar color based on the gradient
            trashBar.color = barColor.Evaluate(fillAmount);
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
