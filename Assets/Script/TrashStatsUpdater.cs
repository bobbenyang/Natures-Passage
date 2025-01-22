using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace


public class TrashStatsUpdater : MonoBehaviour
{
    [Header("Trash Update Settings")]
    public TextMeshProUGUI trashText; // Reference to the UI Text component
    public float baseTrashPerResident = 453f; // Total yearly trash per resident in kilograms
    public float duration = 10f; // Total duration for the display to update (in seconds)
    public float speedIncreaseDuration = 5f; // Duration over which the update speed increases
    public float initialSpeed = 1f; // Initial update speed (days per second)
    public float finalSpeed = 10f; // Final update speed (days per second)

    private float trashPerDay; // Trash per resident per day
    private float elapsedTime; // Tracks time passed
    private int currentDay; // Current day (1 to 365)
    private float displayedTrash; // Total trash displayed

    void Start()
    {
        // Calculate trash per day based on yearly data
        trashPerDay = baseTrashPerResident / 365f;
        currentDay = 1; // Start at day 1
        displayedTrash = trashPerDay; // Initialize with day 1's trash
        elapsedTime = 0f;
    }

    void Update()
    {
        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the current update speed based on elapsed time
        float currentSpeed = Mathf.Lerp(initialSpeed, finalSpeed, Mathf.Clamp01(elapsedTime / speedIncreaseDuration));

        // Update the day count based on the current speed
        currentDay = Mathf.Clamp(Mathf.FloorToInt(elapsedTime * currentSpeed), 1, 365);

        // Calculate the total displayed trash up to the current day
        displayedTrash = currentDay * trashPerDay;

        // Update the UI text
        trashText.text = $"On average, one person in the Netherlands produces {displayedTrash:F2} kg of trash per {currentDay} day(s).";

        // Stop updating after reaching the final duration
        if (elapsedTime >= duration)
        {
            enabled = false; // Disable the script
        }
    }
}

