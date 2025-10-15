using UnityEngine;

public class Campfire : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private GameObject interactionBubble;
  [SerializeField] private GameObject litCampfire;
  [SerializeField] private Dialogue dialogue;
  [SerializeField] private Outline outline;
  [SerializeField] private DeathManager playerDeath;

  private ProximityDetection proxDetect;
  private DialogueManager dialogueManager;
  private bool actived = false;

  private GameObject player;

  private void Start()
  {
    litCampfire.SetActive(false);
    proxDetect = GetComponent<ProximityDetection>();
    outline = GetComponent<Outline>();

    player = GameObject.FindGameObjectWithTag("Player");

    proxDetect.target = player.transform;

    proxDetect.onEnterProximity += ProximityEnterHandler;
    proxDetect.onExitProximity += ProximityExitHandler;

    dialogueManager = FindFirstObjectByType<DialogueManager>();

    interactionBubble.SetActive(false);
  }

  private void Update()
  {
    if (!actived && proxDetect.isTargetInRange)
    {
      if (dialogue == null && Input.GetKeyDown(KeyCode.E))
      {
        HealPlayer();
        LitCampfire();
        CleanupEvents();
      }
      else if (!dialogueManager.isActive && Input.GetKeyDown(KeyCode.E))
        dialogueManager.StartDialogue(dialogue);
      else if (dialogueManager.isActive && Input.GetKeyDown(KeyCode.E))
        dialogueManager.NextLine();
    }
  }

  private void ProximityEnterHandler()
  {
    if (!actived)
    {
      outline.isOutlined = true;

      interactionBubble.SetActive(true);
      dialogueManager.onDialogueEnd += OneTimeEvents;
    }
  }

  private void ProximityExitHandler()
  {
    if (!actived)
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
    playerDeath.IncreaseLifes();
    playerDeath.SetAnchor(transform);
  }

  private void CleanupEvents()
  {
    actived = true;
    dialogueManager.onDialogueEnd -= OneTimeEvents;
  }
}
