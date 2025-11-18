using System.Collections;
using UnityEngine;

public class Campfire : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private GameObject interactionBubble;
  [SerializeField] private GameObject litCampfire;
  [SerializeField] private Dialogue dialogue;
  [SerializeField] private Outline outline;
  [SerializeField] private DeathManager playerDeath;
  [SerializeField] private bool startLit = false;
  [SerializeField] private int increaseTries = 1;

  private ProximityDetection proxDetect;
  private DialogueManager dialogueManager;
  private bool actived = false;

  private GameObject player;

  private void Start()
  {
    proxDetect = GetComponent<ProximityDetection>();
    outline = GetComponent<Outline>();

    player = GameObject.FindGameObjectWithTag("Player");

    proxDetect.target = player.transform;

    proxDetect.onEnterProximity += ProximityEnterHandler;
    proxDetect.onExitProximity += ProximityExitHandler;

    dialogueManager = FindFirstObjectByType<DialogueManager>();

    interactionBubble.SetActive(false);

    if (startLit)
    {
      StartCoroutine(StartLitRoutine());
    }
    else
    {
      litCampfire.SetActive(false);
    }
  }

  private void Update()
  {
    if (!actived && (proxDetect.isTargetInRange || startLit))
    {
      if (dialogue == null && Input.GetKeyDown(KeyCode.E))
      {
        OneTimeEvents();
      }
      else if (!dialogueManager.isActive && Input.GetKeyDown(KeyCode.E))
        dialogueManager.StartDialogue(dialogue);
      else if (dialogueManager.isActive && Input.GetKeyDown(KeyCode.E))
        dialogueManager.NextLine();
    }
  }

  private void ProximityEnterHandler()
  {
    if (!actived && !startLit)
    {
      outline.isOutlined = true;

      interactionBubble.SetActive(true);
      dialogueManager.onDialogueEnd += OneTimeEvents;
    }
  }

  private void ProximityExitHandler()
  {
    if (!actived && !startLit)
    {
      outline.isOutlined = false;

      interactionBubble.SetActive(false);
      dialogueManager.onDialogueEnd -= OneTimeEvents;
    }

    if (dialogueManager.isActive)
      dialogueManager.EndDialogue();
  }

  private void OneTimeEvents()
  {
    HealPlayer();
    IncreaseLifePlayer();
    LitCampfire();
    CleanupEvents();
  }

  private void LitCampfire()
  {
    litCampfire.SetActive(true);
    interactionBubble.SetActive(false);
  }

  private void HealPlayer()
  {
    Health playerHealth = player.GetComponent<Health>();
    playerHealth.FullHeal();
  }

  private void IncreaseLifePlayer()
  {
    playerDeath.IncreaseLifes(increaseTries);
    playerDeath.SetAnchor(transform);
  }

  private void CleanupEvents()
  {
    actived = true;
    dialogueManager.onDialogueEnd -= OneTimeEvents;
  }

  private IEnumerator StartLitRoutine()
  {
    yield return new WaitForSeconds(0.1f);
    dialogueManager.onDialogueEnd += OneTimeEvents;
    LitCampfire();
    dialogueManager.StartDialogue(dialogue);
  }
}
