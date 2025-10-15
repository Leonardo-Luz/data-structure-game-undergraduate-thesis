using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDAudioManager : MonoBehaviour
{
  [Header("UI Click Sound")]
  [SerializeField] private AudioSource clickSound;

  private void Awake()
  {
    if (FindObjectsByType<HUDAudioManager>(FindObjectsSortMode.InstanceID).Length > 1)
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);

    AttachToAllButtons();
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnDestroy()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    AttachToAllButtons();
  }

  private void AttachToAllButtons()
  {
    Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.InstanceID);
    foreach (Button button in buttons)
    {
      button.onClick.RemoveListener(PlayClick);
      button.onClick.AddListener(PlayClick);
    }
  }

  private void PlayClick()
  {
    if (AudioManager.Instance == null || clickSound == null)
      return;

    clickSound.Play();
  }
}

