using UnityEngine;

public class HealthBar : MonoBehaviour
{
  [SerializeField] private Transform fillTransform;
  [SerializeField] private SpriteRenderer fillSprite;
  [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

  [SerializeField] private Color fullColor = Color.green;
  [SerializeField] private Color emptyColor = Color.red;

  private Vector3 initialFillScale;
  private Health target;

  public void AttachTo(Health health)
  {
    target = health;
    if (fillTransform != null)
      initialFillScale = fillTransform.localScale;

    target.OnHealthChanged += UpdateBar;
    UpdateBar(target.GetHP(), target.GetMaxHP());
  }

  void LateUpdate()
  {
    if (target == null) return;
    transform.position = target.transform.position + offset;
  }

  void UpdateBar(int cur, int max)
  {
    if (target == null || fillTransform == null) return;

    float normalized = Mathf.Clamp01((float)cur / max);

    // Scale
    Vector3 scale = initialFillScale;
    scale.x = normalized;
    fillTransform.localScale = scale;

    // Color
    if (fillSprite != null)
      fillSprite.color = Color.Lerp(emptyColor, fullColor, normalized);
  }

  void OnDestroy()
  {
    if (target != null)
      target.OnHealthChanged -= UpdateBar;
  }

  public void setOffset(Vector3 offset)
  {
    this.offset = offset;
  }
}
