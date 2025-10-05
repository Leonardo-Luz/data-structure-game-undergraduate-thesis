using UnityEngine;
using System;

public class Health : MonoBehaviour
{
  [SerializeField] private int maxHP = 100;
  private int currentHP;

  // Events
  public event Action<int, int> OnHealthChanged;
  public event Action OnHealthDecreased;
  public event Action OnDeath;

  private void Awake()
  {
    currentHP = maxHP;
    OnHealthChanged?.Invoke(currentHP, maxHP);
  }

  public void TakeDamage(int amount)
  {
    if (currentHP <= 0) return;

    currentHP = Mathf.Max(currentHP - amount, 0);
    OnHealthChanged?.Invoke(currentHP, maxHP);
    OnHealthDecreased?.Invoke();

    if (currentHP <= 0)
    {
      Die();
    }
  }

  public void Heal(int amount)
  {
    if (currentHP <= 0) return;

    currentHP = Mathf.Min(currentHP + amount, maxHP);
    OnHealthChanged?.Invoke(currentHP, maxHP);
  }

  private void Die()
  {
    OnDeath?.Invoke();
  }

  public void SetMaxHP(int value)
  {
    maxHP = value;
    currentHP = maxHP;
    OnHealthChanged?.Invoke(currentHP, maxHP);
  }

  public int GetHP() => currentHP;
  public int GetMaxHP() => maxHP;
}
