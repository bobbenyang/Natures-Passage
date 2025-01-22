using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSlideshowController : MonoBehaviour
{
    [Header("Images to Move")]
    public RawImage[] images;

    [Header("Target Positions")]
    public Vector3[] targetPositions;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Next Scene")]
    public string nextSceneName;

    private int currentIndex = 0;
    private bool isMoving = false;

    public Button slideshowButton;

    private void Start()
    {
        if (slideshowButton == null)
        {
            Debug.LogError("Slideshow Button is not assigned!");
            return;
        }

        if (images == null || images.Length == 0)
        {
            Debug.LogError("Images array is empty or not assigned!");
            return;
        }

        if (targetPositions == null || targetPositions.Length == 0)
        {
            Debug.LogError("Target Positions array is empty or not assigned!");
            return;
        }

        if (images.Length != targetPositions.Length)
        {
            Debug.LogError("The number of images must match the number of target positions!");
        }

        slideshowButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (!isMoving && currentIndex < images.Length)
        {
            StartCoroutine(MoveImageToPosition(images[currentIndex], targetPositions[currentIndex]));
            currentIndex++;

            if (currentIndex == images.Length)
            {
                Invoke("LoadNextScene", 1f); // Delay before scene transition
            }
        }
    }

    private System.Collections.IEnumerator MoveImageToPosition(RawImage image, Vector3 targetPosition)
    {
        if (image == null)
        {
            Debug.LogError("Image is null. Skipping movement.");
            yield break;
        }

        isMoving = true;
        Vector3 startPosition = image.rectTransform.localPosition;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            image.rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            yield return null;
        }

        image.rectTransform.localPosition = targetPosition;
        isMoving = false;
    }

    private void LoadNextScene()
    {
        slideshowButton.interactable = false; // Disable interactions

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }
}
