using UnityEngine;

public class Turret : MonoBehaviour
{
  [Header("Targeting")]
  [SerializeField] private Transform player;
  [SerializeField] private Transform firePoint;
  [SerializeField] private float eyeFollowRadius = 0.15f;

  [Header("Shooting")]
  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private Vector2 fireRateRange = new Vector2(0.5f, 1.2f);        // (min, max)
  [SerializeField] private Vector2 projectileSpeedRange = new Vector2(6f, 12f);   // (min, max)
  [SerializeField] private bool randomizeStats = true;
  [SerializeField] private ProximityDetection proximity;

  private float fireCooldown = 0f;
  private float projectileSpeed = 10f; // current shot speed
  private Health turretHealth;
  private Vector3 initialFirePointLocalPos;

  void Start()
  {
    if (player == null)
      player = GameObject.FindGameObjectWithTag("Player")?.transform;

    proximity = GetComponent<ProximityDetection>();
    if (proximity != null)
      proximity.target = player;

    turretHealth = GetComponent<Health>();
    if (turretHealth != null)
      turretHealth.OnDeath += OnTurretDestroyed;

    // Initial random stats
    fireCooldown = Random.Range(fireRateRange.x, fireRateRange.y);
    projectileSpeed = Random.Range(projectileSpeedRange.x, projectileSpeedRange.y);

    initialFirePointLocalPos = firePoint.localPosition;
  }

  void Update()
  {
    if (player == null || turretHealth == null) return;

    if (proximity != null && proximity.isTargetInRange)
    {
      // ---- Make the pupil "look" at the player ----
      Vector2 direction = (player.position - firePoint.position).normalized;
      firePoint.localPosition = initialFirePointLocalPos + (Vector3)(direction * eyeFollowRadius);

      // Ensure no unwanted rotation
      firePoint.rotation = Quaternion.identity;

      // ---- Handle shooting ----
      fireCooldown -= Time.deltaTime;
      if (fireCooldown <= 0f && turretHealth.GetHP() > 0)
      {
        Shoot();

        if (randomizeStats)
        {
          fireCooldown = Random.Range(fireRateRange.x, fireRateRange.y);
          projectileSpeed = Random.Range(projectileSpeedRange.x, projectileSpeedRange.y);
        }
        else
        {
          fireCooldown = fireRateRange.x; // fixed fire rate
        }

        // Consume health as ammo
        turretHealth.TakeDamage(1);
      }
    }
    else
    {
      // Return the pupil to center when player is not in range
      firePoint.localPosition = Vector3.Lerp(firePoint.localPosition, initialFirePointLocalPos, Time.deltaTime * 5f);
    }
  }

  void Shoot()
  {
    if (projectilePrefab == null || firePoint == null) return;

    GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
      Vector2 dir = (player.position - firePoint.position).normalized;
      rb.linearVelocity = dir * projectileSpeed;
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
