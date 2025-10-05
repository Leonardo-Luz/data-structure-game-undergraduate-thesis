using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  [SerializeField] private CanvasGroup pausePanel;
  private bool isPaused = false;

  void Start()
  {
    HidePauseMenu();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
      TogglePause();
  }

  public void TogglePause()
  {
    isPaused = !isPaused;

    if (isPaused)
      ShowPauseMenu();
    else
      HidePauseMenu();

    Time.timeScale = isPaused ? 0f : 1f;
  }

  public void ResumeGame()
  {
    isPaused = false;
    HidePauseMenu();
    Time.timeScale = 1f;
  }

  public void QuitGame()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
  }

  private void ShowPauseMenu()
  {
    pausePanel.alpha = 1f;
    pausePanel.interactable = true;
    pausePanel.blocksRaycasts = true;
  }

  private void HidePauseMenu()
  {
    pausePanel.alpha = 0f;
    pausePanel.interactable = false;
    pausePanel.blocksRaycasts = false;
  }
}
