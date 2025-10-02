using UnityEngine;

public class Knockback : MonoBehaviour
{
  [SerializeField] private float knockbackStrength = 10f;
  [SerializeField] private float knockbackDuration = 0.2f;
  private Rigidbody2D rb;
  private bool isKnockedBack = false;

  public bool IsKnockedBack => isKnockedBack;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    if (rb == null)
    {
      Debug.LogError("Rigidbody2D missing on " + gameObject.name);
    }
  }

  public void ApplyKnockback(Transform source)
  {
    if (rb == null) return;

    isKnockedBack = true;

    Vector2 direction = (transform.position - source.position).normalized;
    rb.AddForce(direction * knockbackStrength, ForceMode2D.Impulse);

    Invoke(nameof(EndKnockback), knockbackDuration);
  }

  private void EndKnockback()
  {
    if (rb != null)
    {
      rb.linearVelocity = Vector2.zero;
    }

    isKnockedBack = false;
  }
}
