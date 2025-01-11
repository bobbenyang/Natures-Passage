using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public List<PuzzlePiece> puzzlePieces;
    public GameObject completionUI; // Reference to the UI panel that appears on completion
    public Button nextSceneButton;  // Button to load the next scene
    public string nextSceneName;    // Name of the next scene to load

    private bool puzzleCompleted = false;

    private void Start()
    {
        if (completionUI != null)
            completionUI.SetActive(false); // Hide the completion UI at start

        if (nextSceneButton != null)
            nextSceneButton.gameObject.SetActive(false);  // Hide the button at start

        if (nextSceneButton != null)
            nextSceneButton.onClick.AddListener(LoadNextScene);
    }

    private void Update()
    {
        if (!puzzleCompleted && IsPuzzleComplete())
        {
            ShowCompletionUI();
        }
    }

    private bool IsPuzzleComplete()
    {
        foreach (var piece in puzzlePieces)
        {
            if (Vector2.Distance(piece.rectTransform.anchoredPosition, piece.snapTarget.anchoredPosition) > piece.snapRange)
            {
                return false;  // If any piece isn't in place, return false
            }
        }
        return true;  // All pieces are correctly placed
    }

    private void ShowCompletionUI()
    {
        puzzleCompleted = true;  // Prevent re-checking

        if (completionUI != null)
        {
            completionUI.SetActive(true);  // Show "complete" text
        }

        if (nextSceneButton != null)
        {
            nextSceneButton.gameObject.SetActive(true);  // Show the button now
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
