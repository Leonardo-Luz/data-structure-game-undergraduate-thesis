using UnityEngine;
using UnityEngine.Events;

public class ProximityDetection : MonoBehaviour
{
  [Header("Target to Detect")]
  public Transform target;

  [Header("Detection Settings")]
  public float detectionRadius = 5f;
  public bool detectOnce = false;
  public bool checkEveryFrame = true;

  [Header("Events")]
  public UnityEvent onEnterProximity;
  public UnityEvent onExitProximity;

  private bool isTargetInRange = false;

  void Start()
  {
    if (!checkEveryFrame)
      CheckProximity();
  }

  void Update()
  {
    if (checkEveryFrame)
      CheckProximity();
  }

  public void CheckProximity()
  {
    if (target == null) return;

    float distance = Vector3.Distance(transform.position, target.position);
    bool currentlyInRange = distance <= detectionRadius;

    if (currentlyInRange && !isTargetInRange)
    {
      isTargetInRange = true;
      onEnterProximity?.Invoke();
      if (detectOnce)
        enabled = false;
    }
    else if (!currentlyInRange && isTargetInRange)
    {
      isTargetInRange = false;
      onExitProximity?.Invoke();
    }
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, detectionRadius);
  }
}
