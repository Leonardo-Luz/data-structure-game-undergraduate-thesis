using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
  [Header("Settings")]
  [Tooltip("Time before the projectile becomes tangible")]
  [SerializeField] private float deathTimeout = 0.05f;
  [SerializeField] private float lifetime = 6f;
  [SerializeField] public int damage = 1;

  private void Start()
  {
    StartCoroutine(LifetimeRoutine());
  }

  private IEnumerator LifetimeRoutine()
  {
    yield return new WaitForSeconds(lifetime);
    Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (!(collider.CompareTag("Player") || collider.CompareTag("Turret")))
      StartCoroutine(DeathTimeoutRoutine());
  }

  private IEnumerator DeathTimeoutRoutine()
  {
    yield return new WaitForSeconds(deathTimeout);
    Destroy(gameObject);
  }
}
