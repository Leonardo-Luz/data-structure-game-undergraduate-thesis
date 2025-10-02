using System;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
  public event Action<int> OnEnemyHit;
  private Knockback knockbackPlayer;
  private Flick flick;
  private PlayerCombat playerCombat;

  void Start()
  {
    knockbackPlayer = GetComponent<Knockback>();
    flick = GetComponent<Flick>();
    playerCombat = GetComponent<PlayerCombat>();

    OnEnemyHit += playerCombat.setInvulnerability;
  }

  void OnTriggerStay2D(Collider2D collider)
  {
    if (!playerCombat.isInvulnerable && collider.CompareTag("Enemy"))
    {
      OnEnemyHit?.Invoke(1);
      knockbackPlayer.ApplyKnockback(collider.GetComponent<Transform>());
      flick.StartFlick();
    }
  }
}
