using UnityEngine;

public class Campfire : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private GameObject interactionBubble;
  [SerializeField] private GameObject litCampfire;
  [SerializeField] private Dialogue dialogue;

  private ProximityDetection proxDetect;
  private DialogueManager dialogueManager;
  private bool actived = false;

  private void Start()
  {
    litCampfire.SetActive(false);
    proxDetect = GetComponent<ProximityDetection>();

    proxDetect.target = GameObject.FindGameObjectWithTag("Player").transform;

    proxDetect.onEnterProximity += ProximityEnterHandler;
    proxDetect.onExitProximity += ProximityExitHandler;

    dialogueManager = FindFirstObjectByType<DialogueManager>();

    interactionBubble.SetActive(false);
  }

  private void Update()
  {
    if (!actived && proxDetect.isTargetInRange)
    {
      if (!dialogueManager.isActive && Input.GetKeyDown(KeyCode.E))
        dialogueManager.StartDialogue(dialogue);
      else if (dialogueManager.isActive && Input.GetKeyDown(KeyCode.E))
        dialogueManager.NextLine();
    }
  }

  private void ProximityEnterHandler()
  {
    if (!actived)
    {
      interactionBubble.SetActive(true);
      dialogueManager.onDialogueEnd += HealPlayer;
      dialogueManager.onDialogueEnd += LitCampfire;
      dialogueManager.onDialogueEnd += CleanupEvents;
    }
  }

  private void ProximityExitHandler()
  {
    if (!actived)
    {
      interactionBubble.SetActive(false);
      dialogueManager.onDialogueEnd -= HealPlayer;
      dialogueManager.onDialogueEnd -= LitCampfire;
      dialogueManager.onDialogueEnd -= CleanupEvents;
    }

    if (dialogueManager.isActive)
      dialogueManager.EndDialogue();
  }

  private void LitCampfire()
  {
    litCampfire.SetActive(true);
    interactionBubble.SetActive(false);
  }

  private void HealPlayer()
  {
    Health playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    playerHealth.FullHeal();
  }

  private void CleanupEvents()
  {
    actived = true;
    dialogueManager.onDialogueEnd -= HealPlayer;
    dialogueManager.onDialogueEnd -= LitCampfire;
    dialogueManager.onDialogueEnd -= CleanupEvents;
  }
}
