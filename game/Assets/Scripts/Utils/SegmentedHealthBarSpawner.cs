using UnityEngine;

public class SegmentedHealthBarSpawner : MonoBehaviour
{
  [SerializeField] private GameObject segmentedHealthBarPrefab;
  [SerializeField] public Vector3 offset = new Vector3(0, -1f, 0);
  [SerializeField] public bool dontSpawn = false;
  [SerializeField] private Transform pos;
  [SerializeField] private Transform parent;

  private GameObject barGO;

  void Start()
  {
    if (pos == null) pos = transform;
    if (parent == null) parent = transform;

    Health health = GetComponent<Health>();
    if (dontSpawn || health == null || segmentedHealthBarPrefab == null) return;

    barGO = Instantiate(segmentedHealthBarPrefab, pos.position + offset, Quaternion.identity);
    SegmentedHealthBar bar = barGO.GetComponent<SegmentedHealthBar>();

    bar.transform.SetParent(parent);

    bar.SetOffset(offset);
    bar.AttachTo(health);
  }

  public void DespawnBar()
  {
    if (barGO != null) Destroy(barGO);
  }

  public void ReSetOffset(Vector3 offset)
  {
    barGO.GetComponent<SegmentedHealthBar>().SetOffset(offset);
  }
}
