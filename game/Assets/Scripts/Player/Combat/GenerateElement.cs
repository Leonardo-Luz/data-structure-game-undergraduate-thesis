using UnityEngine;
using System.Collections;

public class GenerateElement : MonoBehaviour
{
  [Header("Inventories")]
  public StackInventory stackInventory;
  public QueueInventory queueInventory;
  public LinkedListInventory linkedListInventory;

  [Header("Settings")]
  public float cooldown = 3f;

  private int inventoryIndex = 0; // 0 = stack, 1 = queue, 2 = linkedlist
  private bool generating = true;

  private void Start()
  {
    stackInventory = GetComponent<StackInventory>();
    queueInventory = GetComponent<QueueInventory>();
    linkedListInventory = GetComponent<LinkedListInventory>();

    StartCoroutine(GenerateRoutine());
  }

  private IEnumerator GenerateRoutine()
  {
    while (generating)
    {
      yield return new WaitForSeconds(cooldown);

      Element element = GetRandomElement();

      switch (inventoryIndex)
      {
        case 0:
          stackInventory.Push(element);
          Debug.Log($"Generated {element} in STACK");
          break;
        case 1:
          queueInventory.Enqueue(element);
          Debug.Log($"Generated {element} in QUEUE");
          break;
        case 2:
          linkedListInventory.Add(element);
          Debug.Log($"Generated {element} in LINKED LIST");
          break;
      }

      // move to next inventory
      inventoryIndex = (inventoryIndex + 1) % 3;
    }
  }

  private Element GetRandomElement()
  {
    int count = System.Enum.GetValues(typeof(Element)).Length;
    return (Element)Random.Range(0, count);
  }
}

