using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
  [Header("Sound Effects")]
  [SerializeField] private AudioSource hurtAudio;
  [SerializeField] private AudioSource healAudio;
  [SerializeField] private AudioSource jumpAudio;
  [SerializeField] private AudioSource landingAudio;
  [SerializeField] private AudioSource failAudio;
  [SerializeField] private AudioSource chargeAudio;
  [SerializeField] private AudioSource insertAudio;

  public void PlayJumpAudio()
  {
    jumpAudio.Play();
  }

  public void PlayLandingAudio()
  {
    landingAudio.Play();
  }

  public void PlayHurtAudio()
  {
    hurtAudio.pitch = Random.Range(0.9f, 1.1f);
    hurtAudio.Play();
  }

  public void PlayHealAudio()
  {
    healAudio.pitch = Random.Range(0.9f, 1.1f);
    healAudio.Play();
  }

  public void PlayFailAudio()
  {
    failAudio.Play();
  }

  public void PlayInsertAudio(float pitch)
  {
    insertAudio.volume = pitch - 0.2f;
    insertAudio.pitch = pitch;
    insertAudio.Play();
  }

  public void PlayChargeAudio()
  {
    chargeAudio.Play();
  }

  public void StopChargeAudio()
  {
    chargeAudio.Stop();
  }
}
