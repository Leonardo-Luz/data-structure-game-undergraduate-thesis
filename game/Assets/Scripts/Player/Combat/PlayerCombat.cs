using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
  public bool isInvulnerable = false;
  private List<Element> castingList = new List<Element>();

  [SerializeField] private float verticalCooldown = 0.2f;
  private float verticalTimer = 0f;

  [SerializeField] private ChargeParticlesController chargeParticles;

  public Grounded grounded;

  private GameObject fallDamageRebound;

  private void Start()
  {
    stackInventory = GetComponent<StackInventory>();
    queueInventory = GetComponent<QueueInventory>();
    linkedListInventory = GetComponent<LinkedListInventory>();

    grounded = GetComponent<Grounded>();
    flick = GetComponent<Flick>();
    health = GetComponent<Health>();

    health.OnHealthDecreased += CleanCasting;
  }

  private void Update()
  {
    if(transform.position.y < -2.6) FallDamage();

    // INFO: Cycle inside linked list elements
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

    // INFO: Locks casting mid-air
    if (!grounded.IsGrounded()) return;

    // INFO: Start casting from inventories
    if (Input.GetKeyDown(KeyCode.Alpha1)) AddElementFromStack();
    if (Input.GetKeyDown(KeyCode.Alpha2)) AddElementFromQueue();
    if (Input.GetKeyDown(KeyCode.Alpha3)) AddElementFromLinkedList();

    // INFO: Finish casting
    if (Input.GetKeyDown(KeyCode.Return)) FinishCasting();
  }

  private void MoveLinkedSelection(int direction)
  {
    if (linkedListInventory.Count == 0) return;

    linkedListInventory.selectedIndex += direction;
    if(linkedListInventory.selectedIndex > linkedListInventory.Count-1) linkedListInventory.selectedIndex = 0;

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
      chargeParticles.StartCasting(0.5f);
    }
  }

  private void AddElementFromQueue()
  {
    if (queueInventory.Count > 0)
    {
      Element element = queueInventory.Dequeue();
      castingList.Add(element);
      isCasting = true;
      chargeParticles.StartCasting(0.5f);
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

      chargeParticles.StartCasting(0.5f);
    }
  }

  private void FinishCasting()
  {
    if (!isCasting) return;

    // Check for combination
    if (castingList.Count >= 2 && castingList[0] == castingList[1])
    {
      ShootProjectile(castingList[0]);
    }
    else
    {
      health.TakeDamage(1);
      flick.StartFlick();
    }

    CleanCasting();
  }

  private void CleanCasting()
  {
    castingList.Clear();
    isCasting = false;
    chargeParticles.StopCasting();
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
      if (health != null)
          health.TakeDamage(1);

      if (flick != null)
          flick.StartFlick();

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
}
