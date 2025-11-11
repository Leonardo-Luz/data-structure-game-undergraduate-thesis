using UnityEngine;
using TMPro;

public class DeathManager : MonoBehaviour
{
  [Header("Dependencies")]
  [SerializeField] private Health health;
  [SerializeField] private Transform player;
  [SerializeField] private TextMeshProUGUI livesTxt;
  [SerializeField] private GameOver gameOver;
  [SerializeField] private PauseMenu pauseMenu;

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
      Score.Instance.AddDeath();
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
    else
    {
      GameObject player = GameObject.FindGameObjectWithTag("Player");

      var moveScript = player.GetComponent<PlayerMove>();
      var combatScript = player.GetComponent<PlayerCombat>();
      var boxColl = player.GetComponent<BoxCollider2D>();

      if (boxColl != null)
        boxColl.enabled = false;
      if (combatScript != null)
        combatScript.enabled = false;
      if (moveScript != null)
        moveScript.enabled = false;
      if (BookController.Instance != null)
        BookController.Instance.enabled = false;
      if (pauseMenu != null)
        pauseMenu.enabled = false;

      player.SetActive(false);

      gameOver.GameOverMenu();
    }
  }

  public void IncreaseLifes()
  {
    lives++;
    livesTxt.text = "x" + lives;
  }

  public void SetAnchor(Transform respawn)
  {
    respawnAnchor.position = new Vector3(respawn.position.x, respawn.position.y + 2, respawn.position.z);
  }
}
