using UnityEngine;

public class SporesDamage : MonoBehaviour
{
  [SerializeField] private float damageCooldown = 1f;
  private float lastDamageTime = -999f;

  void OnParticleCollision(GameObject collider)
  {
    if (!collider.CompareTag("Player")) return;

    if (Time.time - lastDamageTime < damageCooldown)
      return;

    lastDamageTime = Time.time;

    collider.GetComponent<PlayerCollisions>()?.PlayerDamage(collider);
  }
}
