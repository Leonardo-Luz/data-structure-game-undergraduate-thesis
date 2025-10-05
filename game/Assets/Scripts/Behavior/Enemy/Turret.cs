using UnityEngine;

public class Turret : MonoBehaviour
{
  [Header("Targeting")]
  [SerializeField] private Transform player;
  [SerializeField] private Transform firePoint;

  [Header("Shooting")]
  [SerializeField] private GameObject projectilePrefab;

  [SerializeField] private Vector2 fireRateRange = new Vector2(0.5f, 1.2f); // (min, max)
  [SerializeField] private float projectileSpeed = 10f;
  [SerializeField] private ProximityDetection proximity;

  private float fireCooldown = 0f;
  private Health turretHealth;

  void Start()
  {
    if (player == null)
      player = GameObject.FindGameObjectWithTag("Player")?.transform;

    proximity = GetComponent<ProximityDetection>();

    proximity.target = player;

    turretHealth = GetComponent<Health>();
    if (turretHealth != null)
      turretHealth.OnDeath += OnTurretDestroyed;

    fireCooldown = Random.Range(fireRateRange.x, fireRateRange.y);
  }

  void Update()
  {
    if (player == null || turretHealth == null) return;

    // Shoot if cooldown ready
    if (proximity.isTargetInRange)
    {
      // Rotate firepoint toward player
      Vector2 direction = (player.position - firePoint.position).normalized;
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      firePoint.rotation = Quaternion.Euler(0, 0, angle);

      fireCooldown -= Time.deltaTime;
      if (fireCooldown <= 0f && turretHealth.GetHP() > 0)
      {
        Shoot();
        fireCooldown = Random.Range(fireRateRange.x, fireRateRange.y);

        // Consume "ammo" from health
        turretHealth.TakeDamage(1);
      }
    }
  }

  void Shoot()
  {
    if (projectilePrefab == null || firePoint == null) return;

    GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
      rb.linearVelocity = firePoint.right * projectileSpeed;
    }
  }

  void OnTurretDestroyed()
  {
    Destroy(gameObject);
  }

  private void OnDestroy()
  {
    if (turretHealth != null)
      turretHealth.OnDeath -= OnTurretDestroyed;
  }
}
