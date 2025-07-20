using UnityEngine;

public class Grounded : MonoBehaviour
{
  [SerializeField] private Transform groundCheck;
  [SerializeField] private float checkRadius = 0.2f;
  [SerializeField] private LayerMask groundLayer;

  public bool IsGrounded()
  {
    return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
  }

#if UNITY_EDITOR
  private void OnDrawGizmosSelected()
  {
    if (groundCheck == null) return;
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
  }
#endif
}
