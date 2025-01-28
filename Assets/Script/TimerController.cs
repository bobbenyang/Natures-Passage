using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float countdownTime = 60f; // Timer duration
    public TextMeshProUGUI timerText; // Reference to the Timer text
    public TextMeshProUGUI messageText1; // Reference to the first Message text
    public TextMeshProUGUI messageText2; // Reference to the second Message text
    public Button restartButton; // Reference to the Restart button
    public Button secondButton; // Reference to the second button

    public TrashDetector trashDetector; // Reference to the TrashDetector script
    public int trashThreshold = 5; // Threshold to display different messages

    [Header("Messages")]
    [TextArea]
    public string highTrashMessage = "Too much trash! We need to clean up!"; // Message for high trash count
    [TextArea]
    public string lowTrashMessage = "Good job! The area is clean."; // Message for low trash count
    [TextArea]
    public string highTrashDetails = "Trash detected: {count}"; // Detailed message for high trash count
    [TextArea]
    public string lowTrashDetails = "Trash detected: {count}"; // Detailed message for low trash count

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

        // Get the current trash count from TrashDetector
        int trashCount = trashDetector.GetTrashCount();

        // Replace `{count}` with the actual trash count in the messages
        string processedHighTrashMessage = highTrashMessage.Replace("{count}", trashCount.ToString());
        string processedLowTrashMessage = lowTrashMessage.Replace("{count}", trashCount.ToString());
        string processedHighTrashDetails = highTrashDetails.Replace("{count}", trashCount.ToString());
        string processedLowTrashDetails = lowTrashDetails.Replace("{count}", trashCount.ToString());

        // Display messages based on trash count
        if (trashCount >= trashThreshold)
        {
            messageText1.text = processedHighTrashMessage;
            messageText2.text = processedHighTrashDetails;
        }
        else
        {
            messageText1.text = processedLowTrashMessage;
            messageText2.text = processedLowTrashDetails;
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