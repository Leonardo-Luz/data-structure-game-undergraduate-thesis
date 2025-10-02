using System.Collections;
using UnityEngine;

public class GrassAnimation : MonoBehaviour
{
  [Header("Animation")]
  public Animator animator;
  public float duration = 0.6f;
  public float speed = 8f;

  [Header("Fireflies")]
  [SerializeField] private ParticleSystem fireflyParticles;
  [Range(0f, 1f)][SerializeField] private float fireflyChance = 0.1f;

  private bool isMoving = false;
  private float originalSpeed;

  void Start()
  {
    if (animator == null)
      animator = GetComponent<Animator>();

    if (animator != null)
      originalSpeed = animator.speed;

    fireflyParticles = GetComponent<ParticleSystem>();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (!isMoving && other.CompareTag("Player") && animator != null)
    {
      StartCoroutine(TemporarilyBoostSpeed());

      if (fireflyParticles != null && Random.value < fireflyChance)
      {
        fireflyParticles.Play();
      }
    }
  }

  IEnumerator TemporarilyBoostSpeed()
  {
    isMoving = true;
    animator.speed = speed;

    yield return new WaitForSeconds(duration);

    animator.speed = originalSpeed;
    isMoving = false;
  }
}
