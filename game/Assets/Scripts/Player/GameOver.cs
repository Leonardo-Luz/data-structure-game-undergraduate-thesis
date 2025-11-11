using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
  [SerializeField] private string mainMenuScene = "MainMenu";

  void Start()
  {
    gameObject.SetActive(false);
  }

  public void GameOverMenu()
  {
    gameObject.SetActive(true);
  }

  public void MainMenuLoad()
  {
    SceneManager.LoadScene(mainMenuScene);
  }

  public void TryAgain()
  {
    Scene currentScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(currentScene.buildIndex);
  }
}
