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

    [Header("Blink Settings")]
    public Color normalColor = Color.white; // Default text color
    public Color alertColor = Color.red; // Color when blinking
    public float blinkSpeed = 0.5f; // Speed of blinking (seconds per blink)

    private int trashCount = 0;
    private bool isBlinking = false;

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

            if (trashCount > 100)
            {
                if (!isBlinking)
                {
                    StartCoroutine(BlinkText());
                }
            }
            else
            {
                if (isBlinking)
                {
                    StopCoroutine(BlinkText());
                    isBlinking = false;
                    trashCountText.color = normalColor; // Reset to normal color
                }
            }
        }
    }

    private System.Collections.IEnumerator BlinkText()
    {
        isBlinking = true;

        while (true)
        {
            trashCountText.color = alertColor; // Change to alert color
            yield return new WaitForSeconds(blinkSpeed);

            trashCountText.color = normalColor; // Change back to normal color
            yield return new WaitForSeconds(blinkSpeed);
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
