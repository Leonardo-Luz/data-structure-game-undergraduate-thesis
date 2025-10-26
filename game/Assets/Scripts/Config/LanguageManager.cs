using System;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
  public static LanguageManager Instance { get; private set; }

  [SerializeField] private Language language;

  public event Action onLanguageChange;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    LoadSettings();
  }

  public void SetLanguage(Language value)
  {
    language = value;
    PlayerPrefs.SetString("Language", value.ToString());
    onLanguageChange?.Invoke();
  }

  public Language GetLanguage() => this.language;

  public static Language StringToLanguage(string languageString)
  {
    if (Enum.TryParse(languageString, out Language language))
    {
      return language;
    }
    else
    {
      return Language.English;
    }
  }

  private void LoadSettings()
  {
    SetLanguage(StringToLanguage(PlayerPrefs.GetString("Language")));
  }
}

