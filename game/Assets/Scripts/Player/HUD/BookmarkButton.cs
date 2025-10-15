// BookTabButton.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class BookmarkButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Visuals")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color normalColor = new Color(0.25f, 0.2f, 0.3f);
    [SerializeField] private Color selectedColor = new Color(0.9f, 0.4f, 0.5f);

    [Header("Selection look")]
    [Tooltip("slight offset when on overlay")]
    [SerializeField] private Vector2 selectedOffset = new Vector2(8f, 0f);
    [Tooltip("set >1 if you want slight enlarge")]
    [SerializeField] private float scaleWhenSelected = 1.0f;

    // internal state for restoring
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector2 originalAnchoredPosition;
    private Vector3 originalLocalScale;

    private RectTransform rt;
    private bool isSelected = false;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();

        // store defaults
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        originalAnchoredPosition = rt.anchoredPosition;
        originalLocalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BookController.Instance.SelectTab(this);
    }

    // Called by BookController
    public void SetSelected(bool selected, RectTransform overlayParent = null)
    {
        if (isSelected == selected) return;
        isSelected = selected;

        if (selected)
            MoveToOverlay(overlayParent);
        else
            RestoreToOriginal();
    }

    private void MoveToOverlay(RectTransform overlayParent)
    {
        // store current parent/sibling/pos only first time (already stored in Awake but keep it safe)
        originalParent = originalParent ?? transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        originalAnchoredPosition = rt.anchoredPosition;
        originalLocalScale = transform.localScale;

        // parent to overlay (must exist and be above the book background in hierarchy)
        if (overlayParent != null)
            transform.SetParent(overlayParent, worldPositionStays: false);

        // apply visual tweaks
        rt.anchoredPosition += selectedOffset;
        transform.localScale = originalLocalScale * scaleWhenSelected;

        if (backgroundImage != null)
            backgroundImage.color = selectedColor;
    }

    private void RestoreToOriginal()
    {
        // restore parent and sibling index
        transform.SetParent(originalParent, worldPositionStays: false);
        transform.SetSiblingIndex(Mathf.Clamp(originalSiblingIndex, 0, originalParent.childCount));
        rt.anchoredPosition = originalAnchoredPosition;
        transform.localScale = originalLocalScale;

        if (backgroundImage != null)
            backgroundImage.color = normalColor;
    }

    // in case the UI is rebuilt or reset at runtime
    public void ForceReset()
    {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        originalAnchoredPosition = rt.anchoredPosition;
        originalLocalScale = transform.localScale;
        isSelected = false;
        if (backgroundImage != null) backgroundImage.color = normalColor;
    }
}

