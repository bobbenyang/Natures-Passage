using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneStep
    {
        public RectTransform image; // The UI image to move
        public Vector3 targetPosition; // Target position to move to
        public float moveDuration; // Duration of the move
        public float delayBeforeMove; // Delay before starting the move
    }

    public CutsceneStep[] cutsceneSteps;

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        foreach (CutsceneStep step in cutsceneSteps)
        {
            // Wait for the delay before starting this step
            yield return new WaitForSeconds(step.delayBeforeMove);

            // Start moving the image
            yield return StartCoroutine(MoveImage(step.image, step.targetPosition, step.moveDuration));
        }
    }

    private IEnumerator MoveImage(RectTransform image, Vector3 targetPosition, float duration)
    {
        Vector3 initialPosition = image.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            image.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }

        image.localPosition = targetPosition; // Ensure it ends at the exact target position
    }
}