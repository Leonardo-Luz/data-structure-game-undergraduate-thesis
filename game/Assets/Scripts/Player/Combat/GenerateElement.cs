using System.Collections;
using UnityEngine;

public class GenerateElement : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Inventories inventoriesUI;
  [SerializeField] private ChargeParticlesController chargeParticles;
  [SerializeField] private GameObject outOfManaAlert;
  [SerializeField] private PlayerAudioController sfx;
  [SerializeField] private GenericBar manaBar;

  [Header("Settings")]
  [SerializeField] private float cooldown = 1f;
  [SerializeField] public float maxMana = 4.5f;
  [SerializeField] public float ManaRegen = 2f;
  [SerializeField] public float outOfManaDelay = 0.5f;

  [HideInInspector] public float curMana;
  private float delay = 1.5f;
  private bool infiniteMana = false;

  private IInventory[] inventories;
  private int inventoryIndex = 0; // index into inventories[]
  private bool isCharging = false;
  private float chargeTimer = 0f;

  private Grounded grounded;

  public bool isLocked = false;

  private void Start()
  {
    curMana = maxMana;

    inventories = GetComponents<IInventory>();

    if (inventoriesUI == null) inventoriesUI = GetComponent<Inventories>();
    if (chargeParticles == null) chargeParticles = GetComponent<ChargeParticlesController>();
    grounded = GetComponent<Grounded>();

    manaBar.AttachTo(chargeTimer, cooldown);
  }

  private void Update()
  {
    if (isLocked || inventories == null || inventories.Length == 0) return;

    bool holdingDown = Input.GetAxis("Vertical") < 0;

    if (!outOfManaAlert.activeInHierarchy && (curMana < cooldown - 0.5f))
      sfx.PlayFailAudio();

    outOfManaAlert.SetActive(curMana < cooldown - 0.5f);

    if (delay >= outOfManaDelay && curMana > 0 && holdingDown && (grounded == null || grounded.IsGrounded()))
    {
      if (curMana <= 0.1f) delay = 0;

      if (!isCharging)
      {
        sfx.PlayChargeAudio();
        isCharging = true;
        chargeTimer = 0f;
        if (chargeParticles != null) chargeParticles.StartCharging(cooldown);
      }

      chargeTimer += Time.deltaTime;

      manaBar.UpdateBar(chargeTimer, cooldown);

      if (!infiniteMana) curMana -= Time.deltaTime;

      if (chargeTimer >= cooldown)
      {
        if (TryGenerateElement())
        {
          // PLAY GENERATED AUDIO
        }
        else
        {
          delay = 0;
          sfx.StopChargeAudio();
          sfx.PlayFailAudio();
        }
        chargeTimer = 0f;
      }
    }
    else
    {
      delay += Time.deltaTime;

      if (curMana < maxMana)
        curMana += Time.deltaTime * ManaRegen;

      if (isCharging)
      {
        sfx.StopChargeAudio();
        isCharging = false;
        chargeTimer = 0f;
        if (chargeParticles != null) chargeParticles.StopCharging();
      }

      manaBar.UpdateBar(chargeTimer, cooldown);
    }

    UpdateNextSlotPointer();
  }

  private bool TryGenerateElement()
  {
    if (inventories == null || inventories.Length == 0) return false;

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
        return true;
      }

      inventoryIndex = (idx + 1) % inventories.Length;
      attempts++;
      if (inventoryIndex % inventories.Length == startIndex) break;
    }

    return false;
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

  private int prev = -1;
  private int repeat = 0;
  public Element GetRandomElement()
  {
    int count = 0;
    int result = 0;
    do
    {
      count = System.Enum.GetValues(typeof(Element)).Length - 1;
      result = Random.Range(0, count);

      if (result == prev) repeat++;
      else repeat = 0;

      prev = result;
    } while (repeat >= 2);

    return (Element)result;
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

  public IEnumerator InfiniteManaRoutine(float duration)
  {
    infiniteMana = true;
    yield return new WaitForSeconds(duration);

    infiniteMana = false;
  }
}
