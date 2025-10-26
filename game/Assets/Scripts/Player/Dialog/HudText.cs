using TMPro;
using UnityEngine;

public class HudText : MonoBehaviour
{
  [SerializeField] private Dialogue dialogue;
  [SerializeField] private TextMeshProUGUI hudText;

  private void Start()
  {
    hudText = GetComponent<TextMeshProUGUI>();

    LanguageManager.Instance.onLanguageChange += UpdateText;

    if (dialogue != null) UpdateText();
  }

  private void OnDestroy()
  {
    LanguageManager.Instance.onLanguageChange -= UpdateText;
  }

  public void UpdateText()
  {
    DialogueLine line = dialogue.lines[0];
    string text = LanguageManager.Instance.GetLanguage() == Language.English ? line.englishText : line.portugueseText;
    hudText.text = text;
  }

  public void SetDialogue(Dialogue dialogue)
  {
    this.dialogue = dialogue;
    UpdateText();
  }
}
