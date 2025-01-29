using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextCycler : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Reference to the TextMeshPro component
    public Image imageComponent; // Reference to the Image component
    public string[] textArray; // Array of strings to cycle through
    public Sprite[] imageArray; // Array of images to cycle through
    public float displayDuration = 2f; // Time each text stays visible
    public float fadeDuration = 0.5f; // Time for fade in and fade out
    public float transitionDuration = 0.5f; // Time between fading out and the next one appearing

    private int currentIndex = 0;

    private void Start()
    {
        if (textArray.Length > 0 && textComponent != null && imageArray.Length > 0 && imageComponent != null)
        {
            textComponent.alpha = 0f;
            imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 0f);
            StartCoroutine(CycleContent());
        }
        else
        {
            Debug.LogError("TextArray or ImageArray is empty, or TextComponent/ImageComponent is not assigned!");
        }
    }

    private IEnumerator CycleContent()
    {
        while (true)
        {
            // Wait before showing new content
            yield return new WaitForSeconds(transitionDuration);

            // Set the current text and image, then fade them in
            textComponent.text = textArray[currentIndex];
            textComponent.alpha = 0f;
            imageComponent.sprite = imageArray[currentIndex];
            yield return StartCoroutine(FadeInContent());

            // Wait for the display duration
            yield return new WaitForSeconds(displayDuration);

            // Fade out the text and image
            yield return StartCoroutine(FadeOutContent());
        
            // Wait for transition duration before switching to the next content
            yield return new WaitForSeconds(transitionDuration);

            // Move to the next content index
            currentIndex = (currentIndex + 1) % textArray.Length;
        }
    }

    private IEnumerator FadeOutContent()
    {
        float elapsedTime = 0f;
        float startAlpha = textComponent.alpha;
        Color imageColor = imageComponent.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            textComponent.alpha = newAlpha;
            imageComponent.color = new Color(imageColor.r, imageColor.g, imageColor.b, newAlpha);
            yield return null;
        }

        textComponent.alpha = 0f;
        imageComponent.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0f);
    }

    private IEnumerator FadeInContent()
    {
        float elapsedTime = 0f;
        Color imageColor = imageComponent.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            textComponent.alpha = newAlpha;
            imageComponent.color = new Color(imageColor.r, imageColor.g, imageColor.b, newAlpha);
            yield return null;
        }

        textComponent.alpha = 1f;
        imageComponent.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1f);
    }
}
