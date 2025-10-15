using UnityEngine;

public class ProximityToast : MonoBehaviour
{
  [SerializeField] private Dialogue dialogue;
  [SerializeField] private ToastManager toast;

  private ProximityDetection proxDetect;
  private DialogueManager dialogueManager;

  private void Start()
  {
    proxDetect = GetComponent<ProximityDetection>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();

    proxDetect.onEnterProximity += ProximityEnterHandler;
  }

  private void ProximityEnterHandler()
  {
    toast.Toast(dialogue.lines[0]);
    proxDetect.onEnterProximity -= ProximityEnterHandler;
  }
}
