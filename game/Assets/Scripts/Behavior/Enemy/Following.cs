using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowingEnemy : MonoBehaviour
{
  [Header("Follow Settings")]
  [SerializeField] private float speed = 3f;
  [SerializeField] private Transform target;
  [SerializeField] private float followRange = 5f;

  private Rigidbody2D rb;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
  }

  private void FixedUpdate()
  {
    if (target == null) return;

    float distance = Vector2.Distance(transform.position, target.position);

    if (distance <= followRange)
    {
      Vector2 direction = (target.position - transform.position).normalized;
      rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

      if (direction.x > 0 && transform.localScale.x < 0)
        Flip();
      else if (direction.x < 0 && transform.localScale.x > 0)
        Flip();
    }
    else
    {
      rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
  }

  private void Flip()
  {
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, followRange);
  }
}

