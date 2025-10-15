using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class SaveData
{
  public int highestUnlockedLevel = 1;
}

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

  [Header("Scene Names")]
  [SerializeField] private string mainMenuScene = "MainMenu";
  [SerializeField] private string levelMenuScene = "LevelMenu";

  [Tooltip("Add all level scene names here in order")]
  [SerializeField] private string[] levels;

  private SaveData saveData;
  private string savePath;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    savePath = Path.Combine(Application.persistentDataPath, "save.json");
    LoadProgress();
  }

  public void StartGame()
  {
    SceneManager.LoadScene(levelMenuScene);
  }

  public void LoadLevel(int levelIndex)
  {
    if (levelIndex < 0 || levelIndex >= levels.Length) return;
    SceneManager.LoadScene(levels[levelIndex]);
  }

  public void CompleteLevel(int levelIndex)
  {
    if (levelIndex + 1 < levels.Length)
    {
      UnlockLevel(levelIndex + 1);
    }

    SaveProgress();
    SceneManager.LoadScene(levelMenuScene);
  }

  public void UnlockLevel(int levelIndex)
  {
    if (levelIndex + 1 > saveData.highestUnlockedLevel)
    {
      saveData.highestUnlockedLevel = levelIndex + 1;
      SaveProgress();
    }
  }

  public int GetHighestUnlockedLevel()
  {
    return saveData.highestUnlockedLevel;
  }

  public void RestartLevel()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void LoadMainMenu()
  {
    SceneManager.LoadScene(mainMenuScene);
  }

  private void SaveProgress()
  {
    string json = JsonUtility.ToJson(saveData, true);
    File.WriteAllText(savePath, json);
  }

  private void LoadProgress()
  {
    if (File.Exists(savePath))
    {
      string json = File.ReadAllText(savePath);
      saveData = JsonUtility.FromJson<SaveData>(json);
    }
    else
    {
      saveData = new SaveData();
      SaveProgress();
    }
  }

  public void QuitGame()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
  }
}
