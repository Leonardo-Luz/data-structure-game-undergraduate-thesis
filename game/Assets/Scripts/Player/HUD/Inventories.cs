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
  public Color highlightColor = Color.yellow;
  public Color defaultColor = Color.clear;

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
    UpdateInventoryUI(stackInventory.ToArray(), stackSlots, stackBorders, stackInventory.MaxSize, highlight: stackInventory.Count - 1);
    UpdateInventoryUI(queueInventory.ToArray(), queueSlots, queueBorders, queueInventory.MaxSize, highlight: 0, invertVertical: true);
    UpdateInventoryUI(linkedListInventory.ToArray(), linkedSlots, linkedBorders, linkedListInventory.MaxSize, highlight: linkedListInventory.selectedIndex);
  }

  private void UpdateInventoryUI(Element[] elements, Image[] slots, Image[] borders, int maxSize, int highlight, bool invertVertical = false)
  {
    for (int i = 0; i < maxSize; i++)
    {
      int index = i;

      if (invertVertical)
      {
        index = maxSize - 1 - i;
      }

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

    if (elements.Length > 0)
    {
      int highlightIndex = highlight;

      if (invertVertical)
        highlightIndex = maxSize - 1 - highlightIndex;

      borders[highlightIndex].color = highlightColor;
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

