using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrashCollector : MonoBehaviour
{
    // Reference to the TextMeshProUGUI element on the Canvas
    public TextMeshProUGUI trashCountText;

    // Counter for trash collected
    private int trashCount = 0;

    // Called when another object enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the tag "Trash"
        if (other.CompareTag("Trash"))
        {
            // Increment the trash count
            trashCount++;

            // Update the UI text
            UpdateTrashCountUI();

            // Optionally, destroy the trash object after counting
            Destroy(other.gameObject);
        }
    }

    // Update the Canvas text with the current trash count
    private void UpdateTrashCountUI()
    {
        if (trashCountText != null)
        {
            trashCountText.text = $"You have cleaned {trashCount} Trash";
        }
        else
        {
            Debug.LogWarning("TrashCountText is not assigned on " + gameObject.name);
        }
    }
}
