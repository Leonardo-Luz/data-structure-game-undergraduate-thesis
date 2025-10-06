using UnityEngine;
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
  [SerializeField] private CanvasGroup dialogueHud;

  [SerializeField] private Dialogue currentDialogue;
  [SerializeField] public Language currentLanguage = Language.English;

  public event Action onLanguageChange;

  public event Action onDialogueStart;
  public event Action onDialogueEnd;

  [Header("Typing Settings")]
  public float letterDelay = 0.05f;

  private int currentIndex = 0;
  public bool isActive = false;
  private bool isTyping = false;
  private Coroutine typingCoroutine;

  private void Start()
  {
    HideDialogue();
  }

  public void StartDialogue(Dialogue dialogue)
  {
    currentDialogue = dialogue;
    currentIndex = 0;
    isActive = true;
    GameObject.FindGameObjectWithTag("Cam").GetComponent<Fov>().toggleZoom();
    ShowDialogue();
    ShowLine();
    onDialogueStart?.Invoke();
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
    HideDialogue();
    GameObject.FindGameObjectWithTag("Cam").GetComponent<Fov>().toggleZoom();
    onDialogueEnd?.Invoke();
  }

  public void SetLanguage(Language lang)
  {
    currentLanguage = lang;
    onLanguageChange?.Invoke();
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
