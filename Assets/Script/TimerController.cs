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

    private float currentTime;
    private bool isCountingDown = true;

    void Start()
    {
        // Initialize variables
        currentTime = countdownTime;

        // Ensure messages and button are hidden initially
        messageText1.gameObject.SetActive(false);
        messageText2.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        // Assign a function to the button
        restartButton.onClick.AddListener(RestartTimer);
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
        // Hide timer and show the messages and button
        timerText.gameObject.SetActive(false);
        messageText1.gameObject.SetActive(true);
        messageText2.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
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
    }
}
