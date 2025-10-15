using UnityEngine;

public class SpawnTalk : MonoBehaviour
{
  [SerializeField] public Dialogue dialogue;

  private DialogueManager dialogueManager;
  private bool talking = false;

  private void Start()
  {
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    dialogueManager.onDialogueEnd += Cleanup;

    if (dialogue != null)
      Run(dialogue);
  }

  private void Update()
  {
    if (talking && dialogueManager.isActive && Input.GetKeyDown(KeyCode.E)) dialogueManager.NextLine();
  }

  private void Cleanup()
  {
    talking = false;
    dialogueManager.onDialogueEnd -= Cleanup;
    Destroy(gameObject);
  }

  public void Run(Dialogue dialogue)
  {
    this.dialogue = dialogue;

    if (dialogueManager == null)
      dialogueManager = FindFirstObjectByType<DialogueManager>();

    if (!dialogueManager.isActive)
      dialogueManager.StartDialogue(dialogue);

    talking = true;
  }
}
