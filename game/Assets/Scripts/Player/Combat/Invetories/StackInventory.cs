using System.Collections.Generic;
using UnityEngine;

public class StackInventory : MonoBehaviour, IInventory
{
  [SerializeField] public int maxSize = 3;
  private Stack<Element> stack = new Stack<Element>();

  public InventoryType Type => InventoryType.Stack;

  public void Push(Element element)
  {
    if (stack.Count < maxSize)
      stack.Push(element);
  }

  public Element Pop()
  {
    return stack.Count > 0 ? stack.Pop() : default;
  }

  public Element Peek()
  {
    return stack.Count > 0 ? stack.Peek() : default;
  }

  public int Count => stack.Count;
  public int MaxSize => maxSize;

  public void Clear() => stack.Clear();

  public Element[] ToArray()
  {
    Element[] array = stack.ToArray();
    System.Array.Reverse(array);
    return array;
  }

  public bool IsFull() => stack.Count == maxSize;
}
