using UnityEngine;
using TMPro;
using System.Collections;

public enum Language
{
    English,
    Portuguese
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Canvas dialogueHud;

    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private Language currentLanguage = Language.English;

    [Header("Typing Settings")]
    public float letterDelay = 0.05f;

    private int currentIndex = 0;
    public bool isActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        dialogueHud.enabled = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentIndex = 0;
        isActive = true;
        GameObject.FindGameObjectWithTag("Cam").GetComponent<Fov>().toggleZoom();
        dialogueHud.enabled = true;
        ShowLine();
    }

    public void NextLine()
    {
        if (!isActive) return;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            ShowFullLine();
            return;
        }

        currentIndex++;
        if (currentDialogue != null && currentIndex < currentDialogue.lines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowLine()
    {
        DialogueLine line = currentDialogue.lines[currentIndex];
        string text = currentLanguage == Language.English ? line.englishText : line.portugueseText;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(letterDelay);
        }

        isTyping = false;
    }

    private void ShowFullLine()
    {
        DialogueLine line = currentDialogue.lines[currentIndex];
        string text = currentLanguage == Language.English ? line.englishText : line.portugueseText;
        dialogueText.text = text;
        isTyping = false;
    }

    public void EndDialogue()
    {
        isActive = false;
        dialogueText.text = "";
        Debug.Log("Dialogue ended");
        dialogueHud.enabled = false;
        GameObject.FindGameObjectWithTag("Cam").GetComponent<Fov>().toggleZoom();
    }

    public void SetLanguage(Language lang)
    {
        currentLanguage = lang;
    }
}
