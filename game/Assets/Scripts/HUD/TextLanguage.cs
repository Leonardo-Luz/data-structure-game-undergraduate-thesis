using UnityEngine;
using TMPro;

public class TextLanguage : MonoBehaviour
{
  [SerializeField] private Dialogue text;
  private TextMeshProUGUI textUI;

  void Start()
  {
    textUI = GetComponent<TextMeshProUGUI>();

    if(textUI != null)
    {
      ChangeText();
      LanguageManager.Instance.onLanguageChange += ChangeText;
    }
  }

  private void ChangeText() {
    textUI.text = isEnglish() ? text.lines[0].englishText : text.lines[0].portugueseText;
  }

  private bool isEnglish() => LanguageManager.Instance.GetLanguage() == Language.English;
}
