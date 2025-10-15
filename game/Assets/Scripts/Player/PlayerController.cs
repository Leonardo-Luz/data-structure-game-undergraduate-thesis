using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [Header("Sound Effects")]
  [SerializeField] private PlayerAudioController audioController;

  [SerializeField] private DialogueManager dialogueManager;
  [SerializeField] private Inventories inventories;

  private Health health;
  private PlayerCombat playerCombat;
  private PlayerMove playerMove;
  private GenerateElement elementGenerator;

  private void Start()
  {
    health = GetComponent<Health>();
    playerMove = GetComponent<PlayerMove>();
    playerCombat = GetComponent<PlayerCombat>();
    elementGenerator = GetComponent<GenerateElement>();

    health.OnHealthDecreased += audioController.PlayHurtAudio;
    health.OnHealthIncreased += audioController.PlayHealAudio;

    dialogueManager.onDialogueStart += LockPlayer;
    dialogueManager.onDialogueEnd += UnlockPlayer;

    SoftReset();
    UnlockPlayer();
  }

  public void LockPlayer()
  {
    playerCombat.isLocked = true;
    playerMove.isLocked = true;
    elementGenerator.isLocked = true;
  }

  public void UnlockPlayer()
  {
    playerCombat.isLocked = false;
    playerMove.isLocked = false;
    elementGenerator.isLocked = false;
  }

  public void SoftReset()
  {
    health.ResetHP();
    playerCombat.ResetPlayerCombat();
    inventories.ResetInventories();
  }
}
