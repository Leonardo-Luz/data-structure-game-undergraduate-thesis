using UnityEngine;

public class Turret : MonoBehaviour
{
  [Header("Targeting")]
  [SerializeField] private Transform player;
  [SerializeField] private Transform firePoint;
  [SerializeField] private float eyeFollowRadius = 0.15f;

  [Header("Shooting")]
  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private Vector2 fireRateRange = new Vector2(0.5f, 1.2f);
  [SerializeField] private Vector2 projectileSpeedRange = new Vector2(6f, 12f);
  [SerializeField] private Vector2 ammoRange = new Vector2(4, 12);
  [SerializeField] private ProximityDetection range;

  [Header("Settings")]
  [SerializeField] private bool infiniteAmmo = false;
  [SerializeField] private bool randomizeStats = true;
  [SerializeField] private HealthBarSpawner ammoBar;
  [SerializeField] private LayerMask obstructionMask;

  private float fireCooldown = 0f;
  private float projectileSpeed = 10f;
  private Health turretHealth;
  private Vector3 initialFirePointLocalPos;

  void Start()
  {
    if (player == null)
      player = GameObject.FindGameObjectWithTag("Player")?.transform;

    range = GetComponent<ProximityDetection>();
    if (range != null)
      range.target = player;

    turretHealth = GetComponent<Health>();
    if (turretHealth != null)
    {
      if (infiniteAmmo)
      {
        turretHealth.OnHealthDecreased += () => turretHealth.Heal(1);
        ammoBar.dontSpawn = true;
        ammoBar.DespawnBar();
      }
      else if(randomizeStats) turretHealth.SetMaxHP(Mathf.RoundToInt(Random.Range(ammoRange.x, ammoRange.y)));

      turretHealth.OnDeath += OnTurretDestroyed;
    }

    fireCooldown = Random.Range(fireRateRange.x, fireRateRange.y);
    projectileSpeed = Random.Range(projectileSpeedRange.x, projectileSpeedRange.y);

    initialFirePointLocalPos = firePoint.localPosition;
  }

  void Update()
  {
    if (player == null || turretHealth == null) return;

    if (range != null && range.isTargetInRange)
    {
      bool hasLineOfSight = !Physics2D.Linecast(firePoint.position, player.position, obstructionMask);

      if (hasLineOfSight)
      {
        Vector2 direction = (player.position - firePoint.position).normalized;
        firePoint.localPosition = initialFirePointLocalPos + (Vector3)(direction * eyeFollowRadius);

        firePoint.rotation = Quaternion.identity;

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
            fireCooldown = fireRateRange.x;
          }

          turretHealth.TakeDamage(1);
        }
      }
      else
      {
        firePoint.localPosition = Vector3.Lerp(
            firePoint.localPosition,
            initialFirePointLocalPos,
            Time.deltaTime * 5f
        );
      }

#if UNITY_EDITOR
      Debug.DrawLine(firePoint.position, player.position, hasLineOfSight ? Color.green : Color.red);
#endif
    }
    else
    {
      firePoint.localPosition = Vector3.Lerp(
          firePoint.localPosition,
          initialFirePointLocalPos,
          Time.deltaTime * 5f
      );
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
