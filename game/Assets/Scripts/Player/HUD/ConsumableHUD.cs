using UnityEngine;
using UnityEngine.UI;

public class ConsumableHUD : MonoBehaviour
{
  private UseConsumable consumable;

  [Header("Settings")]
  [SerializeField] private Image background;
  [SerializeField] private Image slot;
  [SerializeField] private Image border;

  [Header("Sprites")]
  [SerializeField] private Sprite healSprite;
  [SerializeField] private Sprite insertSprite;
  [SerializeField] private Sprite removeSprite;
  [SerializeField] private Sprite sortSprite;

  private void Start()
  {
    consumable = GetComponent<UseConsumable>();
  }

  private void Update()
  {
    if (consumable.isConsuming) border.enabled = true;
    else
    {
      border.enabled = false;

      switch (consumable.curConsumable)
      {
        case Consumable.NONE:
          emptyHUD();
          break;
        case Consumable.HEAL:
          filledHUD();
          slot.sprite = healSprite;
          break;
        case Consumable.INSERT:
          filledHUD();
          slot.sprite = insertSprite;
          break;
        case Consumable.REMOVE:
          filledHUD();
          slot.sprite = removeSprite;
          break;
        case Consumable.SORT:
          filledHUD();
          slot.sprite = sortSprite;
          break;
      }
    }
  }

  private void emptyHUD()
  {
    if (consumable.isConsuming) return;

    background.enabled = false;
    slot.enabled = false;
  }

  private void filledHUD()
  {
    background.enabled = true;
    slot.enabled = true;
  }
}
