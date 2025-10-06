using UnityEngine.UI;

[System.Serializable]
public class InventoryUI
{
  public string name;           // For debugging / inspector clarity
  public Image[] slots;         // Slot images
  public Image[] borders;       // Border images
  public HudText text;
  public Dialogue dialogue;
}
