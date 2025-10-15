using UnityEngine;
using UnityEngine.UI;

public class AudioMenuManager : MonoBehaviour
{
  [SerializeField] private Slider masterSlider;
  [SerializeField] private Slider musicSlider;
  [SerializeField] private Slider sfxSlider;

  private void Start()
  {
    masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
    musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
    sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

    masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
    musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
    sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

    AudioManager.Instance.SetMasterVolume(masterSlider.value);
    AudioManager.Instance.SetMusicVolume(musicSlider.value);
    AudioManager.Instance.SetSFXVolume(sfxSlider.value);
  }
}
