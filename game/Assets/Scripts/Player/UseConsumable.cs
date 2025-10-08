using System.Collections;
using UnityEngine;

public class UseConsumable : MonoBehaviour
{
  [HideInInspector] public bool isConsuming = false;
  [HideInInspector] public Consumable curConsumable = Consumable.NONE;

  [Header("Settings")]
  [SerializeField] private Health health;
  [SerializeField] private GenerateElement elementGenerator;
  [SerializeField] private PlayerCombat playerCombat;

  public void Update()
  {
    if (isConsuming && Input.GetKeyDown(KeyCode.Q) && curConsumable != Consumable.NONE) isConsuming = false;
    else if (isConsuming) Consume(curConsumable);
    else if (!isConsuming && Input.GetKeyDown(KeyCode.Q) && curConsumable != Consumable.NONE) isConsuming = true;
  }

  private void Consume(Consumable consumable)
  {
    switch (consumable)
    {
      case Consumable.HEAL: Heal(); break;
      case Consumable.INSERT: Insert(); break;
      case Consumable.REMOVE: Remove(); break;
      case Consumable.SORT: Sort(); break;
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
      StartCoroutine(ConsumableClear(0.15f));
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      Element element = elementGenerator.GetRandomElement();
      elementGenerator.AddToInventory(element, 1);
      elementGenerator.AddToInventory(element, 1);
      StartCoroutine(ConsumableClear(0.15f));
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      Element element = elementGenerator.GetRandomElement();
      elementGenerator.AddToInventory(element, 2);
      elementGenerator.AddToInventory(element, 2);
      StartCoroutine(ConsumableClear(0.15f));
    }
  }

  private void Remove()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      playerCombat.RemoveOfInventory(playerCombat.inventories[0], InventoryType.Stack);
      StartCoroutine(ConsumableClear(0.15f));
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      playerCombat.RemoveOfInventory(playerCombat.inventories[1], InventoryType.Queue);
      StartCoroutine(ConsumableClear(0.15f));
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      playerCombat.RemoveOfInventory(playerCombat.inventories[2], InventoryType.LinkedList);
      StartCoroutine(ConsumableClear(0.15f));
    }
  }

  private void Sort()
  {
    foreach (IInventory inv in playerCombat.inventories) inv.Sort();

    StartCoroutine(ConsumableClear(0.15f));
  }

  private IEnumerator ConsumableClear(float delay)
  {
    yield return new WaitForSeconds(delay);
    isConsuming = false;
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
