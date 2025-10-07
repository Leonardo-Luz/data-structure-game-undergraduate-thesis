using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDamage : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private Element[] weaknesses;
  [SerializeField] private float invulnerabilityTimeout = 0.2f;
  [SerializeField] private bool randomizeStats = true;
  [SerializeField] private BoxCollider2D HitBox;
  [SerializeField] private BoxCollider2D HurtBox;
  [SerializeField] private float hitCooldown = 0.1f;
  [SerializeField] private ConsumableObject consumablePrefab;
  [Range(0, 100)][SerializeField] private int dropChance = 20;

  private Health health;
  private ElementsIndicator indicator;

  private Flick flick;

  private float lastHitTime;

  private void Start()
  {
    flick = GetComponent<Flick>();

    health = GetComponent<Health>();
    if (health != null)
    {
      health.OnDeath += DeathHandler;
      health.OnHealthDecreased += FlickHandler;
      health.OnHealthDecreased += InvulnerabilityHandler;
    }

    HurtBox = GetComponent<BoxCollider2D>();
    InvulnerabilityHandler();

    if (randomizeStats)
    {
      int rarity;
      weaknesses = GenerateRandomWeaknesses(out rarity);

      if (health != null)
      {
        int hp = 1;
        switch (rarity)
        {
          case 1: hp = Random.Range(1, 2); break; // Common → 1 HP
          case 2: hp = Random.Range(1, 3); break; // Uncommon → 1-2 HP
          case 3: hp = Random.Range(1, 4); break; // Rare → 1-3 HP
        }
        health.SetMaxHP(hp);
      }
    }

    indicator = GetComponentInChildren<ElementsIndicator>();
    if (indicator != null)
    {
      indicator.followTarget = transform;
      indicator.SetElements(weaknesses);
    }
  }

  private Element[] GenerateRandomWeaknesses(out int rarity)
  {
    // Get all possible elements
    List<Element> rawElements = new List<Element>((Element[])System.Enum.GetValues(typeof(Element)));

    // Remove Invalid Element
    if (rawElements.Contains(Element.NONE)) rawElements.Remove(Element.NONE);

    Element[] allElements = rawElements.ToArray();

    // Pick rarity
    rarity = 1; // default common
    int roll = Random.Range(0, 100); // roll for rarity
    if (roll < 60) rarity = 1;       // 60% chance → Common
    else if (roll < 90) rarity = 2;  // 30% chance → Uncommon
    else rarity = 3;                 // 10% chance → Rare

    // Shuffle elements and pick "rarity" count without duplicates
    List<Element> shuffled = new List<Element>(allElements);
    for (int i = 0; i < shuffled.Count; i++)
    {
      int randIndex = Random.Range(i, shuffled.Count);
      Element temp = shuffled[i];
      shuffled[i] = shuffled[randIndex];
      shuffled[randIndex] = temp;
    }

    return shuffled.GetRange(0, rarity).ToArray();
  }

  private void OnDestroy()
  {
    if (health != null)
    {
      health.OnDeath -= DeathHandler;
      health.OnDeath -= FlickHandler;
      health.OnDeath -= InvulnerabilityHandler;
    }
  }

  private void DeathHandler()
  {
    if (Random.Range(0, 100) < dropChance) Instantiate(consumablePrefab, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }

  private void InvulnerabilityHandler()
  {
    StartCoroutine(InvulnerabilityTimeoutRoutine());
  }

  private IEnumerator InvulnerabilityTimeoutRoutine()
  {
    if (HurtBox != null) HurtBox.enabled = false;
    if (HitBox != null) HitBox.enabled = false;
    yield return new WaitForSeconds(invulnerabilityTimeout);
    if (HurtBox != null) HurtBox.enabled = true;
    if (HitBox != null) HitBox.enabled = true;
  }

  private void FlickHandler()
  {
    if (flick != null) flick.StartFlick();
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.CompareTag("Untagged")) return;

    if (Time.time - lastHitTime < hitCooldown) return;
    lastHitTime = Time.time;

    foreach (var weakness in weaknesses)
    {
      if ((weakness == Element.FIRE && collider.CompareTag("Fire")) ||
          (weakness == Element.WATER && collider.CompareTag("Water")) ||
          (weakness == Element.AIR && collider.CompareTag("Air")) ||
          (weakness == Element.EARTH && collider.CompareTag("Earth")))
      {
        health.TakeDamage(collider.GetComponent<ProjectileController>().damage);
        return;
      }
    }
  }
}
