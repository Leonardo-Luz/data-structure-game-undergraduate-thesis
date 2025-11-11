using UnityEngine;
using System.Collections;

public class LevelFinish : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private Outline outline;
  [SerializeField] private ProximityDetection proxDetect;
  [SerializeField] private GameObject interactionBubble;
  [SerializeField] private ToastManager toast;
  [SerializeField] private Dialogue dialogue;
  [SerializeField] private int levelIndex = 0;
  [SerializeField] private PauseMenu pauseMenu;
  [SerializeField] private LevelMenuManager finishPanel;

  [Header("Audio")]
  [SerializeField] private AudioSource music;
  [SerializeField] private AudioSource sfx;

  private Transform player;
  private bool levelEnding = false;

  private void Start()
  {
    proxDetect = GetComponent<ProximityDetection>();
    outline = GetComponent<Outline>();
    player = GameObject.FindGameObjectWithTag("Player").transform;

    proxDetect.target = player;
    proxDetect.onEnterProximity += ProximityEnterHandler;
    proxDetect.onExitProximity += ProximityExitHandler;

    interactionBubble.SetActive(false);
    outline.isOutlined = false;
  }

  private void Update()
  {
    if (levelEnding) return;

    if (proxDetect.isTargetInRange && Input.GetKeyDown(KeyCode.E))
    {
      levelEnding = true;
      music.Stop();
      sfx.Play();
      StartCoroutine(LevelEndSequence());
    }
  }

  private IEnumerator LevelEndSequence()
  {
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

    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
    rb.gravityScale = 0f;
    float moveDuration = 3f;
    float moveSpeed = 3f;

    float timer = 0f;
    while (timer < moveDuration)
    {
      timer += Time.deltaTime;
      rb.linearVelocity = new Vector2(moveSpeed, 0);
      yield return null;
    }

    rb.linearVelocity = Vector2.zero;

    finishPanel.UpdateEndHUD();
  }

  private void ProximityEnterHandler()
  {
    outline.isOutlined = true;
    interactionBubble.SetActive(true);
  }

  private void ProximityExitHandler()
  {
    outline.isOutlined = false;
    interactionBubble.SetActive(false);
  }

  public void ExitLevel()
  {
    GameManager.Instance.CompleteLevel(levelIndex);
  }
}
