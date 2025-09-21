using UnityEngine;
using UnityEngine.UI;

public class HUDHearts : MonoBehaviour
{
  [SerializeField] private Image[] bottles;
  [SerializeField] private Sprite fullBottleSprite;
  [SerializeField] private Sprite emptyBottleSprite;
  private Health playerHealth;

  private void Start()
  {
    playerHealth = GetComponent<Health>();

    playerHealth.OnHealthChanged += UpdateHearts;
    UpdateHearts(playerHealth.GetHP(), playerHealth.GetMaxHP());
  }

  private void UpdateHearts(int currentHP, int maxHP)
  {
    for (int i = 0; i < bottles.Length; i++)
    {
      Animator anim = bottles[i].GetComponent<Animator>();

      if (i < currentHP)
      {
        bottles[i].sprite = fullBottleSprite;
        if (anim != null) anim.enabled = true;
      }
      else
      {
        bottles[i].sprite = emptyBottleSprite;
        if (anim != null) anim.enabled = false;
      }
    }
  }
}
