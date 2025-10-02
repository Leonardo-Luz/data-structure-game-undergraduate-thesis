using UnityEngine;

public class GenerateElement : MonoBehaviour
{
  [Header("Inventories")]
  public StackInventory stackInventory;
  public QueueInventory queueInventory;
  public LinkedListInventory linkedListInventory;
  public Inventories inventoriesUI; // reference to HUD

  [Header("Settings")]
  [SerializeField] private float cooldown = 3f;
  [SerializeField] private ChargeParticlesController chargeParticles;

  public int inventoryIndex = 0; // 0 = stack, 1 = queue, 2 = linkedlist
  private bool isCharging = false;
  private float chargeTimer = 0f;

  private Grounded grounded;

  private void Start()
  {
    stackInventory = GetComponent<StackInventory>();
    queueInventory = GetComponent<QueueInventory>();
    linkedListInventory = GetComponent<LinkedListInventory>();
    inventoriesUI = GetComponent<Inventories>();

    grounded = GetComponent<Grounded>();
  }

  private void Update()
  {
    bool holdingDown = Input.GetAxis("Vertical") < 0;

    if (holdingDown && grounded.IsGrounded())
    {
      if (!isCharging)
      {
        isCharging = true;
        chargeTimer = 0f;
        chargeParticles.StartCharging(cooldown);
      }

      chargeTimer += Time.deltaTime;

      if (chargeTimer >= cooldown)
      {
        TryGenerateElement();
        chargeTimer = 0f; // reset
      }
    }
    else
    {
      isCharging = false;
      chargeTimer = 0f;
      chargeParticles.StopCharging();
    }

    UpdateNextSlotPointer();
  }

  private void TryGenerateElement()
  {
    int attempts = 0;
    int startIndex = inventoryIndex;

    while (attempts < 3)
    {
      if (CanAddToInventory(inventoryIndex))
      {
        AddToInventory(GetRandomElement(), inventoryIndex);
        inventoryIndex = (inventoryIndex + 1) % 3;
        return;
      }

      inventoryIndex = (inventoryIndex + 1) % 3;
      attempts++;

      if (inventoryIndex == startIndex) break;
    }

    // All inventories full → lock generation
    Debug.Log("All inventories are full! Cannot generate.");
  }

  private bool CanAddToInventory(int index)
  {
    switch (index)
    {
      case 0: return !stackInventory.IsFull();
      case 1: return !queueInventory.IsFull();
      case 2: return !linkedListInventory.IsFull();
    }
    return false;
  }

  private void AddToInventory(Element element, int index)
  {
    switch (index)
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
  }

  private Element GetRandomElement()
  {
    int count = System.Enum.GetValues(typeof(Element)).Length;
    return (Element)Random.Range(0, count);
  }

  private void UpdateNextSlotPointer()
  {
    int target = FindNextAvailableInventory();
    if (target == -1)
    {
      inventoriesUI.nextInventoryIndex = -1;
      inventoriesUI.nextSlotIndex = -1;
      return;
    }

    inventoriesUI.nextInventoryIndex = target;
    inventoriesUI.nextSlotIndex = GetNextSlotIndex(target);
  }

  private int FindNextAvailableInventory()
  {
    for (int i = 0; i < 3; i++)
    {
      int idx = (inventoryIndex + i) % 3;
      if (!IsInventoryFull(idx))
        return idx;
    }
    return -1; // all full
  }

  private bool IsInventoryFull(int idx)
  {
    switch (idx)
    {
      case 0: return stackInventory.Count >= stackInventory.MaxSize;
      case 1: return queueInventory.Count >= queueInventory.MaxSize;
      case 2: return linkedListInventory.Count >= linkedListInventory.MaxSize;
    }
    return true;
  }

  private int GetNextSlotIndex(int idx)
  {
    switch (idx)
    {
      case 0: return stackInventory.Count;        // next push on top
      case 1: return queueInventory.Count;        // next enqueue at end
      case 2: return linkedListInventory.Count;   // append at end
      default: return -1;
    }
  }
}
