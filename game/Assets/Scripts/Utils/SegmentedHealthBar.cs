using UnityEngine;
using System.Collections.Generic;

public class SegmentedHealthBar : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Transform segmentContainer;
  [SerializeField] private GameObject segmentPrefab;
  [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

  [Header("Colors")]
  [SerializeField] private Color fullColor = Color.green;

  private Health target;
  private List<GameObject> segments = new List<GameObject>();

  public void AttachTo(Health health)
  {
    target = health;
    target.OnHealthChanged += UpdateBar;

    CreateSegments(target.GetMaxHP());
    UpdateBar(target.GetHP(), target.GetMaxHP());
  }

  private void CreateSegments(int count)
  {
    foreach (Transform child in segmentContainer)
      Destroy(child.gameObject);
    segments.Clear();

    for (int i = 0; i < count; i++)
    {
      GameObject seg = Instantiate(segmentPrefab, segmentContainer);
      SpriteRenderer rend = seg.GetComponent<SpriteRenderer>();
      if (rend != null) rend.color = fullColor;
      segments.Add(seg);
    }

    RepositionSegments();
  }

  void LateUpdate()
  {
    if (target == null) return;
    transform.position = target.transform.position + offset;
  }

  void UpdateBar(int cur, int max)
  {
    if (segments.Count == 0) return;

    cur = Mathf.Clamp(cur, 0, max);

    for (int i = 0; i < segments.Count; i++)
    {
      bool shouldBeActive = i < cur;
      if (segments[i] != null)
        segments[i].SetActive(shouldBeActive);
    }

    RepositionSegments();
  }

  private void RepositionSegments()
  {
    float spacing = 0.15f;

    List<GameObject> activeSegs = segments.FindAll(s => s != null && s.activeSelf);
    if (activeSegs.Count == 0) return;

    float totalWidth = (activeSegs.Count - 1) * spacing;
    float startX = -totalWidth / 2f;

    for (int i = 0; i < activeSegs.Count; i++)
      activeSegs[i].transform.localPosition = new Vector3(startX + i * spacing, 0f, 0f);
  }

  void OnDestroy()
  {
    if (target != null)
      target.OnHealthChanged -= UpdateBar;
  }

  public void SetOffset(Vector3 offset)
  {
    this.offset = offset;
  }
}
