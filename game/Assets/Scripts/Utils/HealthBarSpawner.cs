using UnityEngine;

public class HealthBarSpawner : MonoBehaviour
{
  [SerializeField] private GameObject healthBarPrefab;
  [SerializeField] private Vector3 offset = new Vector3(0, -1f, 0);

  void Start()
  {
    Health health = GetComponent<Health>();
    if (health == null || healthBarPrefab == null) return;

    GameObject barGO = Instantiate(healthBarPrefab, transform.position + offset, Quaternion.identity);
    HealthBar bar = barGO.GetComponent<HealthBar>();

    bar.transform.SetParent(transform);

    bar.setOffset(offset);
    bar.AttachTo(health);
  }
}
