using System.Collections;
using UnityEngine;

public class HiddenWall : MonoBehaviour
{
  private bool revealing = false;
  [SerializeField] private float fadeDuration = 0.5f;

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (!revealing && collider.CompareTag("Player"))
      StartCoroutine(RevealRoutine());
  }

  private IEnumerator RevealRoutine()
  {
    revealing = true;

    // Gather all renderers (parent + children)
    SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

    float timer = 0f;

    // Store original colors
    Color[] originalColors = new Color[renderers.Length];
    for (int i = 0; i < renderers.Length; i++)
      originalColors[i] = renderers[i].color;

    // Fade all together
    while (timer < fadeDuration)
    {
      timer += Time.deltaTime;
      float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

      for (int i = 0; i < renderers.Length; i++)
      {
        Color c = originalColors[i];
        renderers[i].color = new Color(c.r, c.g, c.b, alpha);
      }

      yield return null;
    }

    // Make sure they're fully invisible
    for (int i = 0; i < renderers.Length; i++)
    {
      Color c = originalColors[i];
      renderers[i].color = new Color(c.r, c.g, c.b, 0f);
    }

    // Now disable the entire object safely
    gameObject.SetActive(false);
  }
}
