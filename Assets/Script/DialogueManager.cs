using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public bool isNPC1;
        [TextArea]
        public string text;
    }

    public GameObject npc1Bubble;
    public TextMeshProUGUI npc1Text;
    public GameObject npc2Bubble;
    public TextMeshProUGUI npc2Text;

    public List<Dialogue> npcDialogues;

    private int currentIndex = 0;
    private bool dialogueActive = true;
    private bool isTextFullyDisplayed = false;
    private Coroutine typingCoroutine;

    public string nextSceneName; // Name of the scene to load

    void Start()
    {
        ShowDialogue();
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (isTextFullyDisplayed)
            {
                AdvanceDialogue();
            }
            else
            {
                SkipToFullText();
            }
        }
    }

    private void ShowDialogue()
    {
        if (currentIndex < npcDialogues.Count)
        {
            var currentDialogue = npcDialogues[currentIndex];

            if (currentDialogue.isNPC1)
            {
                npc1Bubble.SetActive(true);
                npc2Bubble.SetActive(false);
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeText(npc1Text, currentDialogue.text));
            }
            else
            {
                npc1Bubble.SetActive(false);
                npc2Bubble.SetActive(true);
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeText(npc2Text, currentDialogue.text));
            }
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeText(TextMeshProUGUI textComponent, string text)
    {
        textComponent.text = "";
        isTextFullyDisplayed = false;

        foreach (char c in text.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(0.05f); // Adjust typing speed here
        }

        isTextFullyDisplayed = true;
    }

    private void SkipToFullText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            var currentDialogue = npcDialogues[currentIndex];
            if (currentDialogue.isNPC1)
            {
                npc1Text.text = currentDialogue.text;
            }
            else
            {
                npc2Text.text = currentDialogue.text;
            }
            isTextFullyDisplayed = true;
        }
    }

    private void AdvanceDialogue()
    {
        currentIndex++;
        ShowDialogue();
    }

    private void EndDialogue()
    {
        npc1Bubble.SetActive(false);
        npc2Bubble.SetActive(false);
        dialogueActive = false;
        Debug.Log("Dialogue ended.");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
