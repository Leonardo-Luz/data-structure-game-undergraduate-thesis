using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class HUDLanguageManager : MonoBehaviour
{
  [SerializeField] private TMP_Dropdown languages;

  private void Start()
  {
    List<string> languageNames = System.Enum.GetNames(typeof(Language)).ToList();
    languages.ClearOptions();
    languages.AddOptions(languageNames);

    Language currentLang = LanguageManager.Instance.GetLanguage();
    int currentIndex = languageNames.IndexOf(currentLang.ToString());
    languages.value = currentIndex >= 0 ? currentIndex : 0;
    languages.RefreshShownValue();

    languages.onValueChanged.AddListener(OnLanguageChanged);
  }

  private void OnDestroy()
  {
    languages.onValueChanged.RemoveListener(OnLanguageChanged);
  }

  private void OnLanguageChanged(int index)
  {
    string selectedName = languages.options[index].text;
    if (System.Enum.TryParse(selectedName, out Language selectedLanguage))
    {
      LanguageManager.Instance.SetLanguage(selectedLanguage);
    }
  }
}
