using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Parallax : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Parallax Settings")]
    [SerializeField] private float parallaxFactor = 0.05f;
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool lockY = false;

    [Header("Pixel Perfect")]
    [SerializeField] private bool pixelSnap = true;
    [SerializeField] private float pixelsPerUnit = 16f;

    [Header("Layer Sorting")]
    [SerializeField] private string sortingLayer = "Default"; // e.g. "Background", "Midground", "Foreground"
    [SerializeField] private int orderInLayer = 0;

    private Material mat;
    private Vector2 textureOffset;
    private Vector3 initialCamPos;
    private Renderer rend;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        rend = GetComponent<Renderer>();
        mat = rend.material;
        initialCamPos = cameraTransform.position;

        // Apply sorting layer and order
        rend.sortingLayerName = sortingLayer;
        rend.sortingOrder = orderInLayer;

        // Apply initial offset
        transform.position = new Vector3(
          transform.position.x + offset.x,
          transform.position.y + offset.y,
          transform.position.z
        );
    }

    void LateUpdate()
    {
        Vector3 camDelta = cameraTransform.position - initialCamPos;

        // Scrolling texture
        textureOffset.x = camDelta.x * parallaxFactor;
        textureOffset.y = 0; 
        mat.mainTextureOffset = textureOffset;

        // Target position based on camera
        Vector3 targetPos = new Vector3(
            cameraTransform.position.x + offset.x,
            lockY ? transform.position.y : cameraTransform.position.y + offset.y,
            transform.position.z
        );

        // Pixel snapping
        if (pixelSnap)
        {
            targetPos.x = Mathf.Round(targetPos.x * pixelsPerUnit) / pixelsPerUnit;
            targetPos.y = Mathf.Round(targetPos.y * pixelsPerUnit) / pixelsPerUnit;
        }

        transform.position = targetPos;
    }

    // Allow changing sorting at runtime
    public void SetSorting(string layer, int order)
    {
        rend.sortingLayerName = layer;
        rend.sortingOrder = order;
    }
}
