using TMPro;
using UnityEngine;

public class HudText : MonoBehaviour
{
  [SerializeField] private DialogueManager dialogueManager;
  [SerializeField] private Dialogue dialogue;
  [SerializeField] private TextMeshProUGUI hudText;

  private void Start()
  {
    hudText = GetComponent<TextMeshProUGUI>();

    dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    dialogueManager.onLanguageChange += UpdateText;

    if (dialogue != null) UpdateText();
  }

  public void UpdateText()
  {
    DialogueLine line = dialogue.lines[0];
    string text = dialogueManager != null && dialogueManager.currentLanguage == Language.English ? line.englishText : line.portugueseText;
    hudText.text = text;
  }

  public void SetDialogue(Dialogue dialogue)
  {
    this.dialogue = dialogue;
    UpdateText();
  }
}
