using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField] private CanvasGroup exitConfirmation;
  [SerializeField] private CanvasGroup credits;

  [Header("Dependecies")]
  [SerializeField] private OptionsMenuManager options;

  private bool gameStarted = false;

  private void Start()
  {
    if (exitConfirmation != null)
      HideExit();

    if (credits != null)
      HideCredits();
  }

  private void Update()
  {
    if (!gameStarted && Input.GetKeyDown(KeyCode.Escape))
    {
      HideCredits();
      HideExit();
      options.hideOptions();
    }

    if (!gameStarted && Input.GetKeyDown(KeyCode.E))
    {
      gameStarted = true;
      OnPressAnyKey();
    }
  }

  private void OnPressAnyKey()
  {
    GameManager.Instance.StartGame();
  }

  public void Exit()
  {
    GameManager.Instance.QuitGame();
  }

  public void ShowExit()
  {
    exitConfirmation.alpha = 1f;
    exitConfirmation.interactable = true;
    exitConfirmation.blocksRaycasts = true;
  }

  public void HideExit()
  {
    exitConfirmation.alpha = 0f;
    exitConfirmation.interactable = false;
    exitConfirmation.blocksRaycasts = false;
  }

  public void ShowCredits()
  {
    credits.alpha = 1f;
    credits.interactable = true;
    credits.blocksRaycasts = true;
  }

  public void HideCredits()
  {
    credits.alpha = 0f;
    credits.interactable = false;
    credits.blocksRaycasts = false;
  }
}
