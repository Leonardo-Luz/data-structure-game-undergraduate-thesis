using UnityEngine;
using Unity.Cinemachine;

public class Fov : MonoBehaviour
{
    private CinemachineCamera cineCam;
    private CanvasGroup hudCanvasGroup;

    [Header("Zoom Settings")]
    public float zoomedInFov = 2.8125f;
    public float zoomedOutFov = 5.625f;

    [Header("HUD Fade Settings")]
    public float fadeDuration = 0.3f;

    private bool isZoomedOut = true;
    private float fadeVelocity;
    private float targetAlpha;

    void Start()
    {
        cineCam = GetComponent<CinemachineCamera>();
        GameObject hud = GameObject.FindGameObjectWithTag("HUD");

        if (cineCam == null || hud == null)
        {
            Debug.LogError("Missing CinemachineCamera or HUD (tagged 'HUD')");
            enabled = false;
            return;
        }

        hudCanvasGroup = hud.GetComponent<CanvasGroup>();
        if (hudCanvasGroup == null)
        {
            hudCanvasGroup = hud.AddComponent<CanvasGroup>();
        }

        cineCam.Lens.OrthographicSize = zoomedOutFov;
        targetAlpha = 1f;
        hudCanvasGroup.alpha = targetAlpha;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isZoomedOut = !isZoomedOut;
            cineCam.Lens.OrthographicSize = isZoomedOut ? zoomedOutFov : zoomedInFov;
            targetAlpha = isZoomedOut ? 1f : 0f;
        }

        if (hudCanvasGroup != null)
        {
            hudCanvasGroup.alpha = Mathf.MoveTowards(hudCanvasGroup.alpha, targetAlpha, Time.deltaTime / fadeDuration);
            hudCanvasGroup.interactable = hudCanvasGroup.alpha > 0.95f;
            hudCanvasGroup.blocksRaycasts = hudCanvasGroup.alpha > 0.95f;
        }
    }
}
