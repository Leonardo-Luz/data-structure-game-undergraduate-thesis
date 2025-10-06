public enum InventoryType { Stack, Queue, LinkedList }

public interface IInventory
{
  InventoryType Type { get; }
  int Count { get; }
  int MaxSize { get; }
  bool IsFull();
  void Clear();
  Element[] ToArray();
}
