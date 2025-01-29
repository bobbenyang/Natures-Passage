using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Required for scene management

public class ToggleUIOnClick : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textsToToggle; // Array of TextMeshPro texts
    [SerializeField] private RawImage[] imagesToToggle;       // Array of RawImages
    [SerializeField] private string nextSceneName;            // Name of the next scene to load

    private int currentIndex = 0; // Tracks the current text/image being toggled

    void Start()
    {
        // Ensure only the first text and image are active at the start
        for (int i = 0; i < textsToToggle.Length; i++)
        {
            if (textsToToggle[i] != null)
                textsToToggle[i].gameObject.SetActive(i == 0); // Only first text is active

            if (imagesToToggle != null && i < imagesToToggle.Length && imagesToToggle[i] != null)
                imagesToToggle[i].gameObject.SetActive(i == 0); // Only first image is active
        }
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            ToggleNextUI();
        }
    }

    private void ToggleNextUI()
    {
        // Hide the current text and image
        if (textsToToggle[currentIndex] != null)
            textsToToggle[currentIndex].gameObject.SetActive(false);

        if (imagesToToggle[currentIndex] != null)
            imagesToToggle[currentIndex].gameObject.SetActive(false);

        // Move to the next index
        currentIndex++;

        // Show the next text and image, if any
        if (currentIndex < textsToToggle.Length && currentIndex < imagesToToggle.Length)
        {
            if (textsToToggle[currentIndex] != null)
                textsToToggle[currentIndex].gameObject.SetActive(true);

            if (imagesToToggle[currentIndex] != null)
                imagesToToggle[currentIndex].gameObject.SetActive(true);
        }
        else
        {
            // If all text and images have been toggled, load the next scene
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set in the Inspector.");
        }
    }
}
