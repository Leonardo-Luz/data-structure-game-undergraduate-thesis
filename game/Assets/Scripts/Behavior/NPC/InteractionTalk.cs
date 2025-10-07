using UnityEngine;

public class InteractionTalk : MonoBehaviour
{
  public GameObject interactionIcon;
  private Outline outline;
  private ProximityDetection proxDetect;

  [SerializeField] private Dialogue dialogue;

  private DialogueManager dialogueManager;

  private void Start()
  {
    outline = GetComponent<Outline>();
    proxDetect = GetComponent<ProximityDetection>();

    proxDetect.onEnterProximity += ProximityEnterHandler;
    proxDetect.onExitProximity += ProximityExitHandler;

    dialogueManager = FindFirstObjectByType<DialogueManager>();
  }

  private void Update()
  {
    if (outline.isOutlined)
    {
      if (!dialogueManager.isActive && Input.GetKeyDown(KeyCode.E))
        dialogueManager.StartDialogue(dialogue);
      else if (dialogueManager.isActive && Input.GetKeyDown(KeyCode.E))
        dialogueManager.NextLine();
    }
  }

  private void ProximityEnterHandler()
  {
    outline.isOutlined = true;
    interactionIcon.SetActive(true);
  }

  private void ProximityExitHandler()
  {
    outline.isOutlined = false;
    interactionIcon.SetActive(false);

    if (dialogueManager.isActive)
      dialogueManager.EndDialogue();
  }
}

