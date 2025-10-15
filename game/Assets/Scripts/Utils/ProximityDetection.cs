using UnityEngine;
using System;

public class ProximityDetection : MonoBehaviour
{
  [Header("Target to Detect")]
  public Transform target;

  [Header("Detection Settings")]
  public float detectionRadius = 5f;
  public bool detectOnce = false;
  public bool checkEveryFrame = true;
  public bool detectOnAir = true;

  public event Action onEnterProximity;
  public event Action onExitProximity;
  public event Action onStayProximity;

  public bool isTargetInRange = false;

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
    bool currentlyInRange = isValid() && distance <= detectionRadius;

    if (currentlyInRange) onStayProximity?.Invoke();

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

  private bool isValid()
  {
    if(detectOnAir) return true;

    Grounded grounded = target.GetComponent<Grounded>();

    if(grounded == null) return true;

    return grounded.IsGrounded();
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, detectionRadius);
  }
}
