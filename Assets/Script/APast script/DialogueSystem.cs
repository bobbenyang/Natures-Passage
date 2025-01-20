using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public NPCMovement npcMovement; // Reference to NPCMovement script

    [TextArea(3, 10)]
    public string[] dialogueLines;

    private int currentLineIndex = 0;
    private bool isPlayerNear = false;
    private Transform playerTransform; // Reference to the player's position

    void Start()
    {
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueBox.activeInHierarchy)
            {
                StartDialogue();
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    void StartDialogue()
    {
        dialogueBox.SetActive(true);
        currentLineIndex = 0;
        dialogueText.text = dialogueLines[currentLineIndex];
        npcMovement.PauseMovement();
        npcMovement.LookAtPlayer(playerTransform.position); // Make NPC look at the player
    }

    void DisplayNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        npcMovement.ResumeMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerTransform = other.transform; // Store player's position
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            dialogueBox.SetActive(false);
            npcMovement.ResumeMovement(); // Ensure NPC resumes movement if player exits
        }
    }
}
