using UnityEngine;

public class SegmentedHealthBarSpawner : MonoBehaviour
{
  [SerializeField] private GameObject segmentedHealthBarPrefab;
  [SerializeField] private Vector3 offset = new Vector3(0, -1f, 0);
  [SerializeField] public bool dontSpawn = false;

  private GameObject barGO;

  void Start()
  {
    Health health = GetComponent<Health>();
    if (dontSpawn || health == null || segmentedHealthBarPrefab == null) return;

    barGO = Instantiate(segmentedHealthBarPrefab, transform.position + offset, Quaternion.identity);
    SegmentedHealthBar bar = barGO.GetComponent<SegmentedHealthBar>();

    bar.transform.SetParent(transform);

    bar.SetOffset(offset);
    bar.AttachTo(health);
  }

  public void DespawnBar()
  {
    if (barGO != null) Destroy(barGO);
  }
}
