using UnityEngine;

public class GenericBar : MonoBehaviour
{
  [SerializeField] private Transform fillTransform;
  [SerializeField] private SpriteRenderer fillSprite;
  [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

  [SerializeField] private Color fullColor = Color.green;
  [SerializeField] private Color emptyColor = Color.red;
  [SerializeField] private Transform target;

  private Vector3 initialFillScale;

  public void AttachTo(float min, float max)
  {
    if (fillTransform != null)
      initialFillScale = fillTransform.localScale;

    UpdateBar(min, max);
  }

  private void LateUpdate()
  {
    if (target == null) return;
    transform.position = target.transform.position + offset;
  }

  public void UpdateBar(float cur, float max)
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

  public void setOffset(Vector3 offset)
  {
    this.offset = offset;
  }
}
