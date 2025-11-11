using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  [SerializeField] private CanvasGroup pausePanel;
  public bool isPaused = false;
  public bool isPauseMenuOpen = false;

  void Start()
  {
    HidePauseMenu();
    isPauseMenuOpen = false;
    isPaused = false;
    Time.timeScale = 1f;
  }

  void Update()
  {
    if ((isPauseMenuOpen || !isPaused) && Input.GetKeyDown(KeyCode.Escape))
      TogglePause();
  }

  public void ToggleTime()
  {
    isPaused = !isPaused;
    Time.timeScale = isPaused ? 0f : 1f;
  }

  public void TogglePause()
  {
    ToggleTime();
    if (isPaused)
      ShowPauseMenu();
    else
      HidePauseMenu();
  }

  public void ResumeGame()
  {
    isPaused = false;
    HidePauseMenu();
    Time.timeScale = 1f;
  }

  public void MainMenu()
  {
    GameManager.Instance.LoadMainMenu();
  }

  public void ShowPauseMenu()
  {
    isPauseMenuOpen = true;
    pausePanel.alpha = 1f;
    pausePanel.interactable = true;
    pausePanel.blocksRaycasts = true;
  }

  public void HidePauseMenu()
  {
    isPauseMenuOpen = false;
    pausePanel.alpha = 0f;
    pausePanel.interactable = false;
    pausePanel.blocksRaycasts = false;
  }
}
