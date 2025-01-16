using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonTeleporter : MonoBehaviour
{
    // Name of the next scene to load
    [SerializeField]
    private string nextSceneName;

    // Method to load the next scene
    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set in the inspector!");
        }
    }
}
