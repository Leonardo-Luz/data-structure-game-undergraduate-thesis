using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkOpener : MonoBehaviour, IPointerClickHandler
{
  private TextMeshProUGUI text;
  private Camera cam;

  void Awake()
  {
    text = GetComponent<TextMeshProUGUI>();
    var canvas = GetComponentInParent<Canvas>();
    cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, cam);

    if (linkIndex != -1)
    {
      var linkInfo = text.textInfo.linkInfo[linkIndex];

      if (linkInfo.GetLinkID() == "form")
      {
        Application.OpenURL("https://github.com/Leonardo-Luz/data-structure-game-undergraduate-thesis");
      }
    }
  }
}
