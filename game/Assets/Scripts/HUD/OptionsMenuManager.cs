using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
  [Header("Canvas Groups")]
  [SerializeField] private CanvasGroup optionsGroup;
  [SerializeField] private CanvasGroup mainOptionsGroup;
  [SerializeField] private CanvasGroup audioOptionsGroup;
  [SerializeField] private CanvasGroup languageOptionsGroup;

  [Header("Buttons")]
  [SerializeField] private Button volumeButton;
  [SerializeField] private Button languageButton;
  [SerializeField] private Button backButton_Main;
  [SerializeField] private Button backButton_Audio;
  [SerializeField] private Button backButton_Language;

  private void Start()
  {
    // Initial visibility setup
    hideOptions();
    ShowMainOptions();

    // Button listeners
    volumeButton.onClick.AddListener(ShowAudioOptions);
    languageButton.onClick.AddListener(ShowLanguageOptions);
    backButton_Main.onClick.AddListener(CloseOptionsMenu);
    backButton_Audio.onClick.AddListener(ShowMainOptions);
    backButton_Language.onClick.AddListener(ShowMainOptions);
  }

  private void ShowMainOptions()
  {
    SetCanvasGroup(mainOptionsGroup, true);
    SetCanvasGroup(audioOptionsGroup, false);
    SetCanvasGroup(languageOptionsGroup, false);
  }

  private void ShowAudioOptions()
  {
    SetCanvasGroup(mainOptionsGroup, false);
    SetCanvasGroup(audioOptionsGroup, true);
    SetCanvasGroup(languageOptionsGroup, false);
  }

  private void ShowLanguageOptions()
  {
    SetCanvasGroup(mainOptionsGroup, false);
    SetCanvasGroup(audioOptionsGroup, false);
    SetCanvasGroup(languageOptionsGroup, true);
  }

  private void CloseOptionsMenu()
  {
    SetCanvasGroup(optionsGroup, false);
  }

  private void SetCanvasGroup(CanvasGroup cg, bool visible)
  {
    cg.alpha = visible ? 1 : 0;
    cg.interactable = visible;
    cg.blocksRaycasts = visible;
  }

  public void ShowOptions()
  {
    SetCanvasGroup(optionsGroup, true);
  }
  public void hideOptions()
  {
    SetCanvasGroup(optionsGroup, false);
  }
}
