using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToNextScene : MonoBehaviour
{
    // Specify the name of the scene in the Inspector
    [SerializeField] private string sceneName;

    void Update()
    {
        // Check for mouse click or touch input
        if (Input.GetMouseButtonDown(0)) // 0 is for left mouse button
        {
            // Check if the scene name is set and not empty
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("Scene name is not set in the Inspector!");
            }
        }
    }
}

