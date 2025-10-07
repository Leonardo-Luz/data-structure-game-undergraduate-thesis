using System.Collections;
using UnityEngine;

public class ConsumableObject : MonoBehaviour
{
  [SerializeField] private Consumable consumable = Consumable.NONE;
  [SerializeField] private float deathTimeout = 10f;

  [Header("Sprites")]
  [SerializeField] private Sprite healSprite;
  [SerializeField] private Sprite insertSprite;
  [SerializeField] private Sprite removeSprite;
  [SerializeField] private Sprite sortSprite;

  private void Start()
  {
    SetConsumable();
  }

  private void SetConsumable()
  {
    int rarity = 1; // default common
    int roll = Random.Range(0, 100); // roll for rarity
    if (roll < 40) rarity = 1;       // 40% chance → Common
    else if (roll < 70) rarity = 2;  // 30% chance → Common
    else if (roll < 90) rarity = 3;  // 20% chance → Uncommon
    else rarity = 4;                 // 10% chance → Rare

    switch (rarity)
    {
      case 1:
        consumable = Consumable.HEAL;
        GetComponent<SpriteRenderer>().sprite = healSprite;
        break;
      case 2:
        consumable = Consumable.INSERT;
        GetComponent<SpriteRenderer>().sprite = insertSprite;
        break;
      case 3:
        consumable = Consumable.REMOVE;
        GetComponent<SpriteRenderer>().sprite = removeSprite;
        break;
      case 4:
        consumable = Consumable.SORT;
        GetComponent<SpriteRenderer>().sprite = sortSprite;
        break;
    }
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.CompareTag("Player"))
      if (collider.GetComponentInChildren<UseConsumable>().SetConsumable(consumable))
        Destroy(gameObject);
  }

  private IEnumerator DeathTimeoutRoutine()
  {
    yield return new WaitForSeconds(deathTimeout);
    Destroy(gameObject);
  }
}
