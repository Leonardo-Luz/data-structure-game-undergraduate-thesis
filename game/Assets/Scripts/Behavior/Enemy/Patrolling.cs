using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PatrollingEnemy : MonoBehaviour
{
  [Header("Patrol Settings")]
  private float defaultSpeed = 2f;
  [SerializeField] private float speed = 2f;
  [SerializeField] private float staggerDuration = 0.5f;
  [SerializeField] private Transform groundCheck;
  [SerializeField] private LayerMask groundLayer;
  [SerializeField] private float groundCheckDistance = 0.5f;
  [SerializeField] private float wallCheckDistance = 1f;
  [SerializeField] private LayerMask obstacleLayer;

  private Rigidbody2D rb;
  private bool movingRight = true;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    defaultSpeed = speed;
  }

  private void FixedUpdate()
  {
    rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * speed, rb.linearVelocity.y);

    RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    if (groundInfo.collider == null)
    {
      Flip();
    }

    Vector2 direction = movingRight ? Vector2.right : Vector2.left;
    RaycastHit2D wallInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), direction, wallCheckDistance, obstacleLayer);
    if (wallInfo.collider != null)
    {
      Flip();
    }
  }

  private void Flip()
  {
    movingRight = !movingRight;
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
  }

  private void OnDrawGizmosSelected()
  {
    if (groundCheck != null)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
    if (transform.position != null)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + Vector2.right * wallCheckDistance);
    }
  }

  // FIX: collision between player - enemy changed in commit d301b78
  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("Player")) StartCoroutine(StaggerRoutine());
  }

  private IEnumerator StaggerRoutine()
  {
    speed = 0f;
    yield return new WaitForSeconds(staggerDuration);
    speed = defaultSpeed;
  }
}

