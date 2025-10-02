using UnityEngine;
using UnityEngine.UI;

public class Inventories : MonoBehaviour
{
  [Header("Inventories")]
  public StackInventory stackInventory;
  public QueueInventory queueInventory;
  public LinkedListInventory linkedListInventory;

  [Header("HUD Slots (3 for each inventory)")]
  public Image[] stackSlots;
  public Image[] queueSlots;
  public Image[] linkedSlots;

  [Header("Element Sprites")]
  public Sprite fireSprite;
  public Sprite waterSprite;
  public Sprite earthSprite;
  public Sprite airSprite;
  public Sprite emptySprite;

  [Header("Borders")]
  public Image[] stackBorders;
  public Image[] queueBorders;
  public Image[] linkedBorders;
  public Color highlightColor = Color.yellow;   // currently selected
  public Color generationColor = Color.cyan;    // next generation
  public Color defaultColor = Color.clear;

  // values injected from GenerateElement
  [HideInInspector] public int nextInventoryIndex = -1;
  [HideInInspector] public int nextSlotIndex = -1;

  private void Start()
  {
    stackInventory = GetComponent<StackInventory>();
    queueInventory = GetComponent<QueueInventory>();
    linkedListInventory = GetComponent<LinkedListInventory>();
  }

  private void Update()
  {
    RefreshHUD();
  }

  private void RefreshHUD()
  {
    UpdateInventoryUI(stackInventory.ToArray(), stackSlots, stackBorders, stackInventory.MaxSize, stackInventory.Count - 1, 0);
    UpdateInventoryUI(queueInventory.ToArray(), queueSlots, queueBorders, queueInventory.MaxSize, 0, 1, invertVertical: true);
    UpdateInventoryUI(linkedListInventory.ToArray(), linkedSlots, linkedBorders, linkedListInventory.MaxSize, linkedListInventory.selectedIndex, 2);
  }

  private void UpdateInventoryUI(
      Element[] elements,
      Image[] slots,
      Image[] borders,
      int maxSize,
      int highlight,
      int inventoryIndex,
      bool invertVertical = false)
  {
    for (int i = 0; i < maxSize; i++)
    {
      int index = invertVertical ? maxSize - 1 - i : i;

      if (i < elements.Length)
      {
        slots[index].sprite = GetSprite(elements[i]);
        slots[index].color = Color.white;
      }
      else
      {
        slots[index].sprite = emptySprite;
        slots[index].color = Color.clear;
      }

      borders[index].color = defaultColor;
    }

    // highlight (e.g. currently active/selected element)
    if (elements.Length > 0 && highlight >= 0 && highlight < maxSize)
    {
      int highlightIndex = invertVertical ? maxSize - 1 - highlight : highlight;
      borders[highlightIndex].color = highlightColor;
    }

    // show pointer for next generation slot
    if (nextInventoryIndex == inventoryIndex && nextSlotIndex >= 0 && nextSlotIndex < maxSize)
    {
      int pointerIndex = invertVertical ? maxSize - 1 - nextSlotIndex : nextSlotIndex;
      borders[pointerIndex].color = generationColor;
    }
  }

  private Sprite GetSprite(Element element)
  {
    switch (element)
    {
      case Element.FIRE: return fireSprite;
      case Element.WATER: return waterSprite;
      case Element.EARTH: return earthSprite;
      case Element.AIR: return airSprite;
      default: return emptySprite;
    }
  }
}
