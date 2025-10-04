using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
  [SerializeField] private Health health;

  private void Start()
  {
    health.OnDeath += Death;
  }

  private void Death()
  {
    ReloadScene();
  }

  private void ReloadScene()
  {
    Scene currentScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(currentScene.buildIndex);
  }
}
