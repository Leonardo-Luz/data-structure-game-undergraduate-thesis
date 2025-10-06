using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
  [Header("Inventories")]
  private IInventory[] inventories;
  private LinkedListInventory linkedListInventory; // only 1 allowed

  [Header("Projectile Prefabs")]
  public GameObject fireProjectile;
  public GameObject waterProjectile;
  public GameObject earthProjectile;
  public GameObject airProjectile;

  [Header("Combat Settings")]
  public Transform shootPoint;
  public float projectileSpeed = 10f;

  [SerializeField] private float verticalCooldown = 0.2f;
  private float verticalTimer = 0f;

  [SerializeField] private ChargeParticlesController chargeParticles;

  private Flick flick;
  private Health health;
  private ElementsIndicator indicator;
  private Grounded grounded;

  public bool isCasting = false;
  public bool isInvulnerable = false;
  public List<Element> castingList = new List<Element>();

  private GameObject fallDamageRebound;

  private void Start()
  {
    inventories = GetComponents<IInventory>();
    linkedListInventory = GetComponent<LinkedListInventory>();

    flick = GetComponent<Flick>();
    health = GetComponent<Health>();
    grounded = GetComponent<Grounded>();
    indicator = GetComponentInChildren<ElementsIndicator>();

    health.OnHealthDecreased += CleanCasting;
  }

  private void Update()
  {
    indicator.SetElements(castingList);

    if (transform.position.y < -2.6) FallDamage();

    HandleLinkedListSelection();
    HandleCastingInput();
  }

  private void HandleLinkedListSelection()
  {
    if (linkedListInventory == null) return;

    verticalTimer -= Time.deltaTime;
    float vertical = Input.GetAxisRaw("Vertical");
    if (verticalTimer <= 0f)
    {
      if (vertical > 0.1f)
      {
        MoveLinkedSelection(1);
        verticalTimer = verticalCooldown;
      }
    }
  }

  private void HandleCastingInput()
  {
    if (!grounded.IsGrounded()) return;

    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      AddFromInventory(InventoryType.Stack);
      if (!ValidCombination())
      {
        health.TakeDamage(1);
        flick.StartFlick();
        CleanCasting();
      }
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      AddFromInventory(InventoryType.Queue);

      if (!ValidCombination())
      {
        health.TakeDamage(1);
        flick.StartFlick();
        CleanCasting();
      }
    }
    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      AddFromInventory(InventoryType.LinkedList);

      if (!ValidCombination())
      {
        health.TakeDamage(1);
        flick.StartFlick();
        CleanCasting();
      }
    }

    if (Input.GetKeyDown(KeyCode.Return)) FinishCasting();
  }

  private void AddFromInventory(InventoryType type)
  {
    foreach (var inv in inventories)
    {
      if (inv.Type != type) continue;

      Element element = default;
      switch (type)
      {
        case InventoryType.Stack:
          if (inv.Count > 0) element = (inv as StackInventory).Pop();
          break;
        case InventoryType.Queue:
          if (inv.Count > 0) element = (inv as QueueInventory).Dequeue();
          break;
        case InventoryType.LinkedList:
          if (linkedListInventory.Count > 0)
          {
            element = linkedListInventory.Remove(linkedListInventory.selectedIndex);
            linkedListInventory.selectedIndex = 0;
          }
          break;
      }

      castingList.Add(element);
      isCasting = true;
      chargeParticles.StartCasting(0.5f);
    }
  }

  private void MoveLinkedSelection(int direction)
  {
    if (linkedListInventory.Count == 0) return;

    linkedListInventory.selectedIndex += direction;
    if (linkedListInventory.selectedIndex > linkedListInventory.Count - 1)
      linkedListInventory.selectedIndex = 0;

    linkedListInventory.selectedIndex = Mathf.Clamp(linkedListInventory.selectedIndex, 0, linkedListInventory.Count - 1);
  }

  private void FinishCasting()
  {
    if (!isCasting) return;

    if (castingList.Count >= 2 && ValidCombination())
      ShootProjectile(castingList[0]);
    else
    {
      health.TakeDamage(1);
      flick.StartFlick();
    }

    CleanCasting();
  }

  private bool ValidCombination()
  {
    for (int i = 1; i < castingList.Count; i++)
      if (castingList[0] != castingList[i]) return false;

    return true;
  }

  private void CleanCasting()
  {
    castingList.Clear();
    isCasting = false;
    chargeParticles.StopCasting();
  }

  private void ShootProjectile(Element element)
  {
    GameObject prefab = element switch
    {
      Element.FIRE => fireProjectile,
      Element.WATER => waterProjectile,
      Element.EARTH => earthProjectile,
      Element.AIR => airProjectile,
      _ => null,
    };

    if (prefab == null) return;

    GameObject proj = Instantiate(prefab, shootPoint.position, Quaternion.identity);
    proj.GetComponent<ProjectileController>().damage = castingList.Count - 1;

    Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
      float direction = GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
      rb.linearVelocity = transform.right * projectileSpeed * direction;
    }

    Debug.Log($"Shot {element} projectile!");
  }

  // --- Rebound / fall damage ---
  public void SetupRebound()
  {
    DeleteRebound();
    fallDamageRebound = new GameObject("ReboundPoint");
    fallDamageRebound.transform.position = transform.position;
  }

  public void DeleteRebound()
  {
    if (fallDamageRebound != null)
    {
      Destroy(fallDamageRebound);
      fallDamageRebound = null;
    }
  }

  private void Rebound()
  {
    if (fallDamageRebound != null)
      transform.position = fallDamageRebound.transform.position;
  }

  public void FallDamage()
  {
    health?.TakeDamage(1);
    flick?.StartFlick();
    Rebound();
  }

  public void setInvulnerability(int damage)
  {
    StartCoroutine(invulnerability(damage * 0.5f));
  }

  IEnumerator invulnerability(float invTime)
  {
    isInvulnerable = true;
    yield return new WaitForSeconds(invTime);
    isInvulnerable = false;
  }

  public void FlipShootPoint(bool flipX)
  {
    if (shootPoint == null) return;

    Vector3 localPos = shootPoint.localPosition;
    localPos.x = Mathf.Abs(localPos.x) * (flipX ? -1 : 1);
    shootPoint.localPosition = localPos;
  }
}
