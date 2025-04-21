using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float projectileSpeed = 50f;
    public float fireRate = 0.5f;

    private float nextFireTime;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            ProjectileInstantie();
            nextFireTime = Time.time + fireRate;
        }
    }

    void ProjectileInstantie()
    {
        if (projectilePrefab != null && spawnPoint != null)
        {
            bool fliped = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX;
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, projectilePrefab.transform.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = new Vector2(projectileSpeed * (fliped ? -1 : 1), rb.linearVelocity.y);
                Destroy(projectile, 3f);
            }
            else
            {
                Debug.LogError("Projectile prefab is missing a Rigidbody component.");
                Destroy(projectile);
            }
        }
        else
        {
            Debug.LogError("Projectile prefab or spawn point is not assigned.");
        }
    }
}
