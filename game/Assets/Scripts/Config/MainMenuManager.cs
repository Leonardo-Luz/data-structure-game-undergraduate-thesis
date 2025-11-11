using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField] private CanvasGroup exitConfirmation;

  private bool gameStarted = false;

  private void Start()
  {
    if (exitConfirmation != null)
      HideExit();
  }

  private void Update()
  {
    if (!gameStarted && Input.GetKeyDown(KeyCode.E) && !IsMouseInput())
    {
      gameStarted = true;
      OnPressAnyKey();
    }
  }

  private bool IsMouseInput() => Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);

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
}
