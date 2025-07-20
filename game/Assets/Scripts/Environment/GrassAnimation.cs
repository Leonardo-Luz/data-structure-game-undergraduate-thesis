using System.Collections;
using UnityEngine;

public class GrassAnimation : MonoBehaviour
{
  public Animator animator;

  public bool isMoving = false;

  public float duration = 0.6f;

  public float speed = 8f;
  private float originalSpeed;

  void Start()
  {
    animator = gameObject.GetComponent<Animator>();
    if(animator != null)
    {
      originalSpeed = animator.speed;
    }
  }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isMoving && other.CompareTag("Player") && animator != null)
        {
            StartCoroutine(TemporarilyBoostSpeed());
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
