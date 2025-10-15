using UnityEngine;

public class ProximityTalk : MonoBehaviour
{
  private ProximityDetection proxDetect;

  [SerializeField] private Dialogue dialogue;

  private DialogueManager dialogueManager;

  private void Start()
  {
    proxDetect = GetComponent<ProximityDetection>();

    proxDetect.onEnterProximity += ProximityEnterHandler;

    dialogueManager = FindFirstObjectByType<DialogueManager>();
  }

  private void Update()
  {
    if (proxDetect.enabled && proxDetect.isTargetInRange && dialogueManager.isActive && Input.GetKeyDown(KeyCode.E)) dialogueManager.NextLine();
  }

  private void ProximityEnterHandler()
  {
    if (!dialogueManager.isActive) dialogueManager.StartDialogue(dialogue);
  }
}
