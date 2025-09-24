using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  [SerializeField] private Canvas pausePanel;
  private bool isPaused = false;

  void Start()
  {
    pausePanel.enabled = false;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
      TogglePause();
  }

  public void TogglePause()
  {
    isPaused = !isPaused;
    pausePanel.enabled = isPaused;

    Time.timeScale = isPaused ? 0f : 1f;
  }

  public void ResumeGame()
  {
    isPaused = false;
    pausePanel.enabled = false;
    Time.timeScale = 1f;
  }

  public void QuitGame()
  {
    // For editor
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
  }
}

