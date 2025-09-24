using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
  [SerializeField] private Element[] weaknesses;
  private Health health;

  private void Start()
  {
    health = GetComponent<Health>();
    if (health != null)
    {
      health.OnDeath += DeathHandler;
    }
  }

  private void OnDestroy()
  {
    if (health != null)
      health.OnDeath -= DeathHandler;
  }

  private void DeathHandler()
  {
    Destroy(gameObject);
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    for (int i = 0; i < weaknesses.Length; i++)
    {
      switch (weaknesses[i])
      {
        case Element.FIRE:
          if (collider.CompareTag("Fire")) health.TakeDamage(1);
          break;
        case Element.WATER:
          if (collider.CompareTag("Water")) health.TakeDamage(1);
          break;
        case Element.AIR:
          if (collider.CompareTag("Air")) health.TakeDamage(1);
          break;
        case Element.EARTH:
          if (collider.CompareTag("Earth")) health.TakeDamage(1);
          break;
      }
    }
  }
}
