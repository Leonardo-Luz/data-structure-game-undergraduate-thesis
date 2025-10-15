using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConsumableObject : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private Consumable consumable = Consumable.NONE;
  [SerializeField] private bool despawn = true;
  [SerializeField] private float despawnTimeout = 10f;
  [SerializeField] private AudioSource sfx;

  [Header("Sprites")]
  [SerializeField] private Sprite healSprite;
  [SerializeField] private Sprite insertSprite;
  [SerializeField] private Sprite removeSprite;
  [SerializeField] private Sprite manaSprite;
  [SerializeField] private Sprite sortSprite;
  [SerializeField] private Sprite lifeSprite;

  public event Action onPickup;

  private bool picked = false;

  private void Start()
  {
    if (consumable == Consumable.NONE) SetConsumable();

    if (despawn) StartCoroutine(DeathTimeoutRoutine(despawnTimeout));

    switch (consumable)
    {
      case Consumable.HEAL:
        GetComponent<SpriteRenderer>().sprite = healSprite;
        break;
      case Consumable.INSERT:
        GetComponent<SpriteRenderer>().sprite = insertSprite;
        break;
      case Consumable.REMOVE:
        GetComponent<SpriteRenderer>().sprite = removeSprite;
        break;
      case Consumable.MANA:
        GetComponent<SpriteRenderer>().sprite = manaSprite;
        break;
      case Consumable.SORT:
        GetComponent<SpriteRenderer>().sprite = sortSprite;
        break;
      case Consumable.LIFE:
        GetComponent<SpriteRenderer>().sprite = lifeSprite;
        break;
    }
  }

  private void SetConsumable()
  {
    int rarity = 1; // default common
    int roll = Random.Range(0, 100); // roll for rarity

    int first = 30;
    int second = first + 25;
    int third = second + 20;
    int fourth = third + 10;
    int fifth = fourth + 10;
    int sixth = third + 5;

    if (roll < first) rarity = 1;
    else if (roll < second) rarity = 2;
    else if (roll < third) rarity = 3;
    else if (roll < fourth) rarity = 4;
    else if (roll < fifth) rarity = 5;
    else if (roll < sixth) rarity = 6;

    switch (rarity)
    {
      case 1:
        consumable = Consumable.HEAL;
        break;
      case 2:
        consumable = Consumable.INSERT;
        break;
      case 3:
        consumable = Consumable.REMOVE;
        break;
      case 4:
        consumable = Consumable.MANA;
        break;
      case 5:
        consumable = Consumable.SORT;
        break;
      case 6:
        consumable = Consumable.LIFE;
        break;
    }
  }

  void OnTriggerStay2D(Collider2D collider)
  {
    if (!picked && collider.CompareTag("Player"))
      if (collider.GetComponentInChildren<UseConsumable>().SetConsumable(consumable))
      {
        picked = true;

        sfx.Play();

        onPickup?.Invoke();

        if (onPickup == null)
          StartCoroutine(DeathTimeoutRoutine(0.2f));
      }
  }


  private IEnumerator DeathTimeoutRoutine(float delay)
  {
    yield return new WaitForSeconds(delay);
    Destroy(gameObject);
  }
}
