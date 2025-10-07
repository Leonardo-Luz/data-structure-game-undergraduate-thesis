using System.Collections.Generic;
using UnityEngine;

public class QueueInventory : MonoBehaviour, IInventory
{
  [SerializeField] public int maxSize = 3;
  private Queue<Element> queue = new Queue<Element>();

  public InventoryType Type => InventoryType.Queue;

  public void Enqueue(Element element)
  {
    if (queue.Count < maxSize)
      queue.Enqueue(element);
  }

  public Element Dequeue()
  {
    return queue.Count > 0 ? queue.Dequeue() : default;
  }

  public Element Peek()
  {
    return queue.Count > 0 ? queue.Peek() : default;
  }

  public int Count => queue.Count;
  public int MaxSize => maxSize;

  public void Clear() => queue.Clear();

  public Element[] ToArray() => queue.ToArray();

  public bool IsFull() => queue.Count == maxSize;

  public void Sort()
  {
    List<Element> elements = new List<Element>(queue);

    elements.Sort();

    queue.Clear();
    foreach (var element in elements) queue.Enqueue(element);
  }
}
