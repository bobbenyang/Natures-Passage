using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerControlR : MonoBehaviour
{
    public float countdownTime = 60f; // Timer duration
    public TextMeshProUGUI timerText; // Reference to the Timer text
    public TextMeshProUGUI messageText1; // Reference to the first Message text
    public TextMeshProUGUI messageText2; // Reference to the second Message text
    public Button restartButton; // Reference to the Restart button
    public Button secondButton; // Reference to the second button

    public int scoreThreshold = 5; // Threshold to display different messages

    [Header("Messages")]
    [TextArea]
    public string highScoreMessage = "Great work! Your score is high!"; // Message for high score
    [TextArea]
    public string lowScoreMessage = "Keep trying! Your score is low."; // Message for low score
    [TextArea]
    public string highScoreDetails = "Score: {score}"; // Detailed message for high score
    [TextArea]
    public string lowScoreDetails = "Score: {score}"; // Detailed message for low score

    private float currentTime;
    private bool isCountingDown = true;

    void Start()
    {
        // Initialize variables
        currentTime = countdownTime;

        // Ensure messages and buttons are hidden initially
        messageText1.gameObject.SetActive(false);
        messageText2.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        secondButton.gameObject.SetActive(false);

        // Assign functions to the buttons
        restartButton.onClick.AddListener(RestartTimer);
        secondButton.onClick.AddListener(SecondButtonAction);
    }

    void Update()
    {
        if (isCountingDown)
        {
            currentTime -= Time.deltaTime; // Decrease time

            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountingDown = false;
                ShowEndScreen();
            }

            // Update the timer text (only the countdown)
            timerText.text = Mathf.Ceil(currentTime).ToString();
        }
    }

    void ShowEndScreen()
    {
        // Hide timer and show the messages and buttons
        timerText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        secondButton.gameObject.SetActive(true);

        // Get the current score from GameManager
        int currentScore = GameManager.Instance.GetScore();

        // Replace `{score}` with the actual score in the messages
        string processedHighScoreMessage = highScoreMessage.Replace("{score}", currentScore.ToString());
        string processedLowScoreMessage = lowScoreMessage.Replace("{score}", currentScore.ToString());
        string processedHighScoreDetails = highScoreDetails.Replace("{score}", currentScore.ToString());
        string processedLowScoreDetails = lowScoreDetails.Replace("{score}", currentScore.ToString());

        // Display messages based on the score
        if (currentScore >= scoreThreshold)
        {
            messageText1.text = processedHighScoreMessage;
            messageText2.text = processedHighScoreDetails;
        }
        else
        {
            messageText1.text = processedLowScoreMessage;
            messageText2.text = processedLowScoreDetails;
        }

        messageText1.gameObject.SetActive(true);
        messageText2.gameObject.SetActive(true);
    }

    void RestartTimer()
    {
        // Reset the timer and UI
        currentTime = countdownTime;
        isCountingDown = true;
        timerText.gameObject.SetActive(true);
        messageText1.gameObject.SetActive(false);
        messageText2.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        secondButton.gameObject.SetActive(false);
    }

    void SecondButtonAction()
    {
        // Define what the second button does here
        Debug.Log("Second button clicked!");
    }
}
