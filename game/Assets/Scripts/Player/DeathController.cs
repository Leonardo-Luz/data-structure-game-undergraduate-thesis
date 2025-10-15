using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathManager : MonoBehaviour
{
  [Header("Dependencies")]
  [SerializeField] private Health health;
  [SerializeField] private Transform player;
  [SerializeField] private TextMeshProUGUI livesTxt;

  [Header("Settings")]
  [SerializeField] private Transform respawnAnchor;
  [SerializeField] public int lives = 1;

  private void Start()
  {
    if (health == null)
      health = player.GetComponent<Health>();

    health.OnDeath += Death;

    livesTxt.text = "x" + lives;
  }

  private void Death()
  {
    if (lives > 0)
    {
      lives--;
      livesTxt.text = "x" + lives;

      foreach (Arena arena in FindObjectsByType<Arena>(FindObjectsSortMode.None))
        arena.ArenaReset();

      player.gameObject.SetActive(true);
      player.position = respawnAnchor.position;

      PlayerController controller = player.GetComponent<PlayerController>();
      if (controller != null)
        controller.SoftReset();
    }
    else ReloadScene();
  }

  public void IncreaseLifes()
  {
    lives++;
    livesTxt.text = "x" + lives;
  }

  private void ReloadScene()
  {
    Scene currentScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(currentScene.buildIndex);
  }

  public void SetAnchor(Transform respawn)
  {
    respawnAnchor.position = new Vector3(respawn.position.x, respawn.position.y + 2, respawn.position.z);
  }
}
