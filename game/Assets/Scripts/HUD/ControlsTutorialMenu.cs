using System.Collections;
using UnityEngine;

public class ControlsTutorialMenu : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private CanvasGroup controlsPanel;

  private DialogueManager dialogueManager;
  private PlayerController player;

  private bool open = false;

  private void Start()
  {
    controlsPanel.alpha = 0f;
    controlsPanel.interactable = false;
    controlsPanel.blocksRaycasts = false;

    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    dialogueManager = FindFirstObjectByType<DialogueManager>();


    if(PlayerPrefs.GetInt("ControlTutorial") != 1)
      dialogueManager.onDialogueEnd += ControlsHandler;
  }

  private void Update()
  {
    if(open && (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Escape))) HideControls();
    else if(!open && (Input.GetKeyDown(KeyCode.C))) ShowControls();
  }

  private void ControlsHandler()
  {
    StartCoroutine(ControlsRoutine());
  }

  private IEnumerator ControlsRoutine()
  {
    yield return new WaitForSeconds(0.1f);
    ShowControls();
  }

  private void ShowControls()
  {
    PlayerPrefs.SetInt("ControlTutorial", 1);
    open = true;
    player.LockPlayer();
    controlsPanel.alpha = 1f;
    controlsPanel.interactable = true;
    controlsPanel.blocksRaycasts = true;
  }

  private void HideControls()
  {
    open = false;
    player.UnlockPlayer();
    controlsPanel.alpha = 0f;
    controlsPanel.interactable = false;
    controlsPanel.blocksRaycasts = false;

    dialogueManager.onDialogueEnd -= ControlsHandler;
  }
}
