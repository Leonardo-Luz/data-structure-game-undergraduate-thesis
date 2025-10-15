using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private Button[] levelButtons;

  private void Start()
  {
    int unlocked = GameManager.Instance.GetHighestUnlockedLevel();

    for (int i = 0; i < levelButtons.Length; i++)
    {
      int index = i;
      levelButtons[i].interactable = (i < unlocked);
      levelButtons[i].onClick.AddListener(() => GameManager.Instance.LoadLevel(index));
    }
  }

  public void OnBackPressed()
  {
    Debug.Log("Main Menu Loaded");
    GameManager.Instance.LoadMainMenu();
  }
}
