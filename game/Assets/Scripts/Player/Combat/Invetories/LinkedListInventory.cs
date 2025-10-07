using System.Collections.Generic;
using UnityEngine;

public class LinkedListInventory : MonoBehaviour, IInventory
{
  [SerializeField] private int maxSize = 3;
  private LinkedList<Element> linkedList = new LinkedList<Element>();

  public int selectedIndex = 0;
  public InventoryType Type => InventoryType.LinkedList;

  public void Add(Element element)
  {
    if (linkedList.Count < maxSize)
      linkedList.AddLast(element);
  }

  public Element Remove(int index)
  {
    if (index < 0 || index >= linkedList.Count) return default;

    var node = linkedList.First;
    for (int i = 0; i < index; i++)
      node = node.Next;

    Element element = node.Value;
    linkedList.Remove(node);
    return element;
  }

  public Element RemoveFirst()
  {
    if (linkedList.Count == 0) return default;
    Element element = linkedList.First.Value;
    linkedList.RemoveFirst();
    return element;
  }

  public Element PeekFirst()
  {
    return linkedList.Count > 0 ? linkedList.First.Value : default;
  }

  public int Count => linkedList.Count;
  public int MaxSize => maxSize;

  public void Clear() => linkedList.Clear();

  public Element[] ToArray()
  {
    Element[] array = new Element[linkedList.Count];
    linkedList.CopyTo(array, 0);
    return array;
  }

  public bool IsFull() => linkedList.Count == maxSize;

  public void Sort()
  {
    List<Element> elements = new List<Element>(linkedList);

    elements.Sort();

    linkedList.Clear();
    foreach (var element in elements) linkedList.AddLast(element);

    selectedIndex = Mathf.Clamp(selectedIndex, 0, linkedList.Count - 1);
  }
}
