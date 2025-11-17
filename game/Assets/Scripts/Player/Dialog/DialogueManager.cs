using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public enum Language
{
    English,
    Portuguese
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image dialogueInteraction;
    [SerializeField] private CanvasGroup dialogueHud;
    [SerializeField] private AudioSource dialogueAudio;

    [SerializeField] private Dialogue currentDialogue;

    public event Action onDialogueStart;
    public event Action onDialogueEnd;

    [Header("Typing Settings")]
    public float letterDelay = 0.05f;

    private int totalVisible = 0;
    private int currentIndex = 0;
    public bool isActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        HideDialogue();
    }

    public void StartDialogue(Dialogue dialogue, bool fov = true)
    {
        currentDialogue = dialogue;
        currentIndex = 0;
        isActive = true;
        if (fov) GameObject.FindGameObjectWithTag("Cam").GetComponent<Fov>().toggleZoom();
        else GameObject.FindGameObjectWithTag("Cam").GetComponent<Fov>().RemoveHUD();
        ShowDialogue();
        ShowLine();
        onDialogueStart?.Invoke();
    }

    public void NextLine()
    {
        if (!isActive) return;

        if (isTyping)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
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
        string text = LanguageManager.Instance.GetLanguage() == Language.English ? line.englishText : line.portugueseText;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    public IEnumerator TypeLine(string fullText)
    {
        isTyping = true;
        dialogueInteraction.enabled = false;

        dialogueText.text = fullText;
        dialogueText.ForceMeshUpdate();

        dialogueText.maxVisibleCharacters = 0;
        totalVisible = dialogueText.textInfo.characterCount;

        if (totalVisible <= 0)
            totalVisible = fullText.Length;

        if (dialogueAudio != null) dialogueAudio.Play();

        for (int i = 0; i < totalVisible; i++)
        {
            dialogueText.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(letterDelay);
        }

        if (dialogueAudio != null) dialogueAudio.Stop();

        isTyping = false;
        dialogueInteraction.enabled = true;
        typingCoroutine = null;
    }

    private void ShowFullLine()
    {
        dialogueText.ForceMeshUpdate();
        totalVisible = Mathf.Max(1, dialogueText.textInfo.characterCount);

        dialogueText.maxVisibleCharacters = totalVisible;

        isTyping = false;
        dialogueInteraction.enabled = true;

        if (dialogueAudio != null && dialogueAudio.isPlaying)
            dialogueAudio.Stop();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    public void EndDialogue()
    {
        isActive = false;
        dialogueText.text = "";
        HideDialogue();
        GameObject.FindGameObjectWithTag("Cam").GetComponent<Fov>().RemoveZoom();
        onDialogueEnd?.Invoke();
    }

    private void ShowDialogue()
    {
        dialogueHud.alpha = 1f;
        dialogueHud.interactable = true;
        dialogueHud.blocksRaycasts = true;
    }

    private void HideDialogue()
    {
        dialogueHud.alpha = 0f;
        dialogueHud.interactable = false;
        dialogueHud.blocksRaycasts = false;
    }
}
