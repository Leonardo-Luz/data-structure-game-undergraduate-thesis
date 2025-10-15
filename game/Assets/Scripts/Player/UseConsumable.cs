using System.Collections;
using UnityEngine;

public class UseConsumable : MonoBehaviour
{
  [HideInInspector] public bool isConsuming = false;
  [HideInInspector] public bool consumade = false;
  [HideInInspector] public Consumable curConsumable = Consumable.NONE;

  [Header("Settings")]
  [SerializeField] private Health health;
  [SerializeField] private GenerateElement elementGenerator;
  [SerializeField] private PlayerCombat playerCombat;
  [SerializeField] private DeathManager deathManager;

  public void Update()
  {
    if (isConsuming && Input.GetKeyDown(KeyCode.Q) && curConsumable != Consumable.NONE) isConsuming = false;
    else if (isConsuming && !consumade) Consume(curConsumable);
    else if (!isConsuming && Input.GetKeyDown(KeyCode.Q) && curConsumable != Consumable.NONE) isConsuming = true;
  }

  private void Consume(Consumable consumable)
  {
    switch (consumable)
    {
      case Consumable.HEAL: Heal(); break;
      case Consumable.INSERT: Insert(); break;
      case Consumable.REMOVE: Remove(); break;
      case Consumable.MANA: Mana(); break;
      case Consumable.SORT: Sort(); break;
      case Consumable.LIFE: Life(); break;
    }
  }

  private void Heal()
  {
    health.Heal(1);
    isConsuming = false;
    curConsumable = Consumable.NONE;
  }

  private void Insert()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      if (elementGenerator.IsInventoryFull(0))
      {
        Debug.Log("STACKOVERFLOW");
      }

      Element element = elementGenerator.GetRandomElement();
      elementGenerator.AddToInventory(element, 0);
      elementGenerator.AddToInventory(element, 0);
      ConsumableClear();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      Element element = elementGenerator.GetRandomElement();
      elementGenerator.AddToInventory(element, 1);
      elementGenerator.AddToInventory(element, 1);
      ConsumableClear();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      Element element = elementGenerator.GetRandomElement();
      elementGenerator.AddToInventory(element, 2);
      elementGenerator.AddToInventory(element, 2);
      ConsumableClear();
    }
  }

  private void Remove()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      playerCombat.RemoveOfInventory(playerCombat.inventories[0], InventoryType.Stack);
      ConsumableClear();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      playerCombat.RemoveOfInventory(playerCombat.inventories[1], InventoryType.Queue);
      ConsumableClear();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      playerCombat.RemoveOfInventory(playerCombat.inventories[2], InventoryType.LinkedList);
      ConsumableClear();
    }
  }

  private void Sort()
  {
    foreach (IInventory inv in playerCombat.inventories) inv.Sort();

    ConsumableClear();
  }

  private void Mana()
  {
    StartCoroutine(elementGenerator.InfiniteManaRoutine(6f));
    ConsumableClear();
  }

  private void Life()
  {
    deathManager.lives++;
    ConsumableClear();
  }

  private void ConsumableClear()
  {
    consumade = true;
    StartCoroutine(ConsumableClearRoutine(0.05f));
  }

  private IEnumerator ConsumableClearRoutine(float delay)
  {
    yield return new WaitForSeconds(delay);
    isConsuming = false;
    consumade = false;
    curConsumable = Consumable.NONE;
  }

  public bool SetConsumable(Consumable consumable)
  {
    if (curConsumable == Consumable.NONE)
    {
      curConsumable = consumable;
      return true;
    }

    return false;
  }
}
