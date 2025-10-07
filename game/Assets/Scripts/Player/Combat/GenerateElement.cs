using UnityEngine;

public class GenerateElement : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Inventories inventoriesUI;
  [SerializeField] private ChargeParticlesController chargeParticles;

  [Header("Settings")]
  [SerializeField] private float cooldown = 1f;

  private IInventory[] inventories;
  private int inventoryIndex = 0; // index into inventories[]
  private bool isCharging = false;
  private float chargeTimer = 0f;

  private Grounded grounded;

  private void Start()
  {
    inventories = GetComponents<IInventory>();

    if (inventoriesUI == null) inventoriesUI = GetComponent<Inventories>();
    if (chargeParticles == null) chargeParticles = GetComponent<ChargeParticlesController>();
    grounded = GetComponent<Grounded>();
  }

  private void Update()
  {
    if (inventories == null || inventories.Length == 0) return;

    bool holdingDown = Input.GetAxis("Vertical") < 0;

    if (holdingDown && (grounded == null || grounded.IsGrounded()))
    {
      if (!isCharging)
      {
        isCharging = true;
        chargeTimer = 0f;
        if (chargeParticles != null) chargeParticles.StartCharging(cooldown);
      }

      chargeTimer += Time.deltaTime;

      if (chargeTimer >= cooldown)
      {
        TryGenerateElement();
        chargeTimer = 0f;
      }
    }
    else
    {
      if (isCharging)
      {
        isCharging = false;
        chargeTimer = 0f;
        if (chargeParticles != null) chargeParticles.StopCharging();
      }
    }

    UpdateNextSlotPointer();
  }

  private void TryGenerateElement()
  {
    if (inventories == null || inventories.Length == 0) return;

    int attempts = 0;
    int startIndex = inventoryIndex % inventories.Length;

    while (attempts < inventories.Length)
    {
      int idx = inventoryIndex % inventories.Length;
      var inv = inventories[idx];
      if (inv != null && !inv.IsFull())
      {
        AddToInventory(GetRandomElement(), idx);
        inventoryIndex = (idx + 1) % inventories.Length;
        return;
      }

      inventoryIndex = (idx + 1) % inventories.Length;
      attempts++;
      if (inventoryIndex % inventories.Length == startIndex) break;
    }

    Debug.Log("All inventories are full! Cannot generate.");
  }

  public void AddToInventory(Element element, int index)
  {
    if (inventories == null || index < 0 || index >= inventories.Length) return;

    var inv = inventories[index];
    if (inv == null) return;

    switch (inv.Type)
    {
      case InventoryType.Stack:
        (inv as StackInventory)?.Push(element);
        Debug.Log($"Generated {element} in STACK");
        break;
      case InventoryType.Queue:
        (inv as QueueInventory)?.Enqueue(element);
        Debug.Log($"Generated {element} in QUEUE");
        break;
      case InventoryType.LinkedList:
        (inv as LinkedListInventory)?.Add(element);
        Debug.Log($"Generated {element} in LINKED LIST");
        break;
    }
  }

  public Element GetRandomElement()
  {
    int count = System.Enum.GetValues(typeof(Element)).Length - 1;
    return (Element)Random.Range(0, count);
  }

  private void UpdateNextSlotPointer()
  {
    if (inventoriesUI == null)
      return;

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
    if (inventories == null || inventories.Length == 0) return -1;

    for (int i = 0; i < inventories.Length; i++)
    {
      int idx = (inventoryIndex + i) % inventories.Length;
      var inv = inventories[idx];
      if (inv != null && !inv.IsFull())
        return idx;
    }
    return -1;
  }

  public bool IsInventoryFull(int idx)
  {
    if (inventories == null || idx < 0 || idx >= inventories.Length) return true;
    var inv = inventories[idx];
    return inv == null || inv.IsFull();
  }

  private int GetNextSlotIndex(int idx)
  {
    if (inventories == null || idx < 0 || idx >= inventories.Length) return -1;
    var inv = inventories[idx];
    if (inv == null) return -1;

    // For all inventory types here we append at the end, so next slot is Count
    return inv.Count;
  }
}
