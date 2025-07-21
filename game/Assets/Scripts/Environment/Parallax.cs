using UnityEngine;

public class Parallax : MonoBehaviour
{
  [SerializeField] private Transform cameraTransform;
  [SerializeField] private float parallaxFactor = 0.05f;
  [SerializeField] private Vector2 offset;
  [SerializeField] private bool lockY = false;
  [SerializeField] private bool pixelSnap = true;
  [SerializeField] private float pixelsPerUnit = 16f;

  private Material mat;
  private Vector2 textureOffset;
  private Vector3 initialCamPos;

  void Start()
  {
    if (cameraTransform == null)
      cameraTransform = Camera.main.transform;

    mat = GetComponent<Renderer>().material;
    initialCamPos = cameraTransform.position;

    transform.position = new Vector3(
      transform.position.x + offset.x,
      transform.position.y + offset.y,
      transform.position.z
    );
  }

  void LateUpdate()
  {
    Vector3 camDelta = cameraTransform.position - initialCamPos;

    textureOffset.x = camDelta.x * parallaxFactor;
    textureOffset.y = 0; //camDelta.y * parallaxFactor;
    mat.mainTextureOffset = textureOffset;

    Vector3 targetPos = new Vector3(
        cameraTransform.position.x + offset.x,
        lockY ? transform.position.y : cameraTransform.position.y + offset.y,
        transform.position.z
    );

    if (pixelSnap)
    {
      targetPos.x = Mathf.Round(targetPos.x * pixelsPerUnit) / pixelsPerUnit;
      targetPos.y = Mathf.Round(targetPos.y * pixelsPerUnit) / pixelsPerUnit;
    }

    transform.position = targetPos;
  }
}
