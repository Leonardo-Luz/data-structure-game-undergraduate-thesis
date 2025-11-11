using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance { get; private set; }

  [Header("Mixer")]
  [SerializeField] private AudioMixer mainMixer;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    LoadSettings();
  }

  public void SetMasterVolume(float value)
  {
    mainMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
    PlayerPrefs.SetFloat("MasterVolume", value);
  }

  public void SetMusicVolume(float value)
  {
    mainMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
    PlayerPrefs.SetFloat("MusicVolume", value);
  }

  public void SetSFXVolume(float value)
  {
    mainMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
    PlayerPrefs.SetFloat("SFXVolume", value);
  }

  private void LoadSettings()
  {
    SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1f));
    SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
    SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
  }
}
