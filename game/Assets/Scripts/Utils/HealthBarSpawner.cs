using UnityEngine;

public class HealthBarSpawner : MonoBehaviour
{
  [SerializeField] private GameObject healthBarPrefab;
  [SerializeField] private Vector3 offset = new Vector3(0, -1f, 0);
  [SerializeField] public bool dontSpawn = false;

  private GameObject barGO;

  void Start()
  {
    Health health = GetComponent<Health>();
    if (dontSpawn || health == null || healthBarPrefab == null) return;

    barGO = Instantiate(healthBarPrefab, transform.position + offset, Quaternion.identity);
    HealthBar bar = barGO.GetComponent<HealthBar>();

    bar.transform.SetParent(transform);

    bar.setOffset(offset);
    bar.AttachTo(health);
  }

  public void DespawnBar()
  {
    if (barGO != null) Destroy(barGO);
  }
}
