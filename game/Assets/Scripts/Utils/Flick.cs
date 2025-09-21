using UnityEngine;
using System.Collections;

public class Flick : MonoBehaviour
{
    [Tooltip("The material to revert to after the flick.")]
    public Material OriginalMaterial;
    [Tooltip("The white material used for the flick effect.")]
    public Material WhiteMaterial;
    [Tooltip("The duration of the flick effect in seconds.")]
    public float FlickDuration = 0.1f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject.");
            enabled = false;
            return;
        }

        if (OriginalMaterial == null)
        {
            OriginalMaterial = spriteRenderer.material;
        }

        if (WhiteMaterial == null)
        {
            WhiteMaterial = new Material(Shader.Find("Sprites/FlickShader"));
            WhiteMaterial.color = Color.white;
        }
    }

    public void StartFlick()
    {
        StopAllCoroutines();
        StartCoroutine(FlickRoutine());
    }

    private IEnumerator FlickRoutine()
    {
        spriteRenderer.material = WhiteMaterial;
        yield return new WaitForSeconds(FlickDuration);
        spriteRenderer.material = OriginalMaterial;
    }
}
