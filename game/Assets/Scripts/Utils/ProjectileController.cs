using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
  [Header("Settings")]
  [Tooltip("Time before the projectile becomes tangible")]
  [SerializeField] private float tangibilityTimeout = 0.03f;
  [SerializeField] private float deathTimeout = 0.05f;
  [SerializeField] private float lifetime = 6f;
  [SerializeField] public int damage = 1;

  private bool tangible = false;
  private BoxCollider2D boxCollider;

  private void Start()
  {
    StartCoroutine(TangibilityTimeoutRoutine());
    StartCoroutine(LifetimeRoutine());

    boxCollider = gameObject.GetComponent<BoxCollider2D>();
    CheckTangibility();
  }

  private void Update()
  {
    CheckTangibility();
  }

  private void CheckTangibility()
  {
    if (tangible && !boxCollider.enabled) boxCollider.enabled = true;
    else if (!tangible && boxCollider.enabled) boxCollider.enabled = false;
  }

  private IEnumerator TangibilityTimeoutRoutine()
  {
    yield return new WaitForSeconds(tangibilityTimeout);
    tangible = true;
  }

  private IEnumerator LifetimeRoutine()
  {
    yield return new WaitForSeconds(lifetime);
    Destroy(gameObject);
  }

  private void OnTriggerEnter2D()
  {
    StartCoroutine(DeathTimeoutRoutine());
  }

  private IEnumerator DeathTimeoutRoutine()
  {
    yield return new WaitForSeconds(deathTimeout);
    Destroy(gameObject);
  }
}
