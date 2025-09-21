using System;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
  public event Action<int> OnEnemyHit;
  private Knockback knockbackPlayer;
  private Flick flick;

  void Start()
  {
    knockbackPlayer = GetComponent<Knockback>();
    flick = GetComponent<Flick>();
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("Enemy"))
    {
      OnEnemyHit?.Invoke(1);
      knockbackPlayer.ApplyKnockback(collision.collider.GetComponent<Transform>());
      flick.StartFlick();
    }
  }
}
