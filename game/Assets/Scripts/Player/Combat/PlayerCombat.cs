using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
  [Header("Inventories")]
  public StackInventory stackInventory;
  public QueueInventory queueInventory;
  public LinkedListInventory linkedListInventory;

  [Header("Projectile Prefabs")]
  public GameObject fireProjectile;
  public GameObject waterProjectile;
  public GameObject earthProjectile;
  public GameObject airProjectile;

  private Flick flick;

  [Header("Combat Settings")]
  public Transform shootPoint;
  public float projectileSpeed = 10f;

  private Health health;

  public bool isCasting = false;
  private List<Element> castingList = new List<Element>();

  [SerializeField] private float verticalCooldown = 0.2f;
  private float verticalTimer = 0f;

  private void Start()
  {
    stackInventory = GetComponent<StackInventory>();
    queueInventory = GetComponent<QueueInventory>();
    linkedListInventory = GetComponent<LinkedListInventory>();

    flick = GetComponent<Flick>();

    health = GetComponent<Health>();
  }

  private void Update()
  {
    // Start casting from inventories
    if (Input.GetKeyDown(KeyCode.Alpha1)) AddElementFromStack();
    if (Input.GetKeyDown(KeyCode.Alpha2)) AddElementFromQueue();
    if (Input.GetKeyDown(KeyCode.Alpha3)) AddElementFromLinkedList();

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

    // Finish casting
    if (Input.GetKeyDown(KeyCode.Return))
    {
      FinishCasting();
    }
  }

  private void MoveLinkedSelection(int direction)
  {
    if (linkedListInventory.Count == 0) return;

    linkedListInventory.selectedIndex += direction;
    linkedListInventory.selectedIndex = Mathf.Clamp(linkedListInventory.selectedIndex, 0, linkedListInventory.Count - 1);
  }

  public void flipShootPoint(bool flipX)
  {
    Vector3 localPos = shootPoint.localPosition;
    localPos.x = Mathf.Abs(localPos.x) * (flipX ? -1 : 1);
    shootPoint.localPosition = localPos;
  }

  private void AddElementFromStack()
  {
    if (stackInventory.Count > 0)
    {
      Element element = stackInventory.Pop();
      castingList.Add(element);
      isCasting = true;
      Debug.Log("Casting from Stack: " + element);
    }
  }

  private void AddElementFromQueue()
  {
    if (queueInventory.Count > 0)
    {
      Element element = queueInventory.Dequeue();
      castingList.Add(element);
      isCasting = true;
      Debug.Log("Casting from Queue: " + element);
    }
  }

  private void AddElementFromLinkedList()
  {
    if (linkedListInventory.Count > 0)
    {
      int index = linkedListInventory.selectedIndex;
      Element element = linkedListInventory.Remove(index);
      castingList.Add(element);
      isCasting = true;

      linkedListInventory.selectedIndex = 0;

      Debug.Log("Casting from LinkedList: " + element);
    }
  }

  private void FinishCasting()
  {
    if (!isCasting) return;

    Debug.Log("Finishing cast with elements: " + string.Join(", ", castingList));

    // Check for combination
    if (castingList.Count >= 2 && castingList[0] == castingList[1])
    {
      ShootProjectile(castingList[0]);
    }
    else
    {
      health.TakeDamage(1);
      flick.StartFlick();
      Debug.Log("Invalid combination, player took damage!");
    }

    castingList.Clear();
    isCasting = false;
  }

  private void ShootProjectile(Element element)
  {
    GameObject prefab = null;

    switch (element)
    {
      case Element.FIRE: prefab = fireProjectile; break;
      case Element.WATER: prefab = waterProjectile; break;
      case Element.EARTH: prefab = earthProjectile; break;
      case Element.AIR: prefab = airProjectile; break;
    }

    if (prefab != null)
    {
      GameObject proj = Instantiate(prefab, shootPoint.position, Quaternion.identity);
      Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
      if (rb != null)
      {
        float direction = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
        rb.linearVelocity = transform.right * projectileSpeed * direction;
      }

      Debug.Log($"Shot {element} projectile!");
    }
  }
}
