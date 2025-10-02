using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ChargeParticlesController : MonoBehaviour
{
  private ParticleSystem ps;
  private ParticleSystem.EmissionModule emission;

  [Header("Charge Settings")]
  public float maxEmission = 60f;   // max particles per second
  public float minEmission = 20f;     // starting particles per second
  public float chargeDuration = 3f;  // full charge time (same as cooldown)

  private float chargeTimer = 0f;
  private bool isCharging = false;

  void Awake()
  {
    ps = GetComponent<ParticleSystem>();
    emission = ps.emission;
    ps.Stop();
  }

  void Update()
  {
    if (isCharging)
    {
      chargeTimer += Time.deltaTime;
      float t = Mathf.Clamp01(chargeTimer / chargeDuration);
      emission.rateOverTime = Mathf.Lerp(minEmission, maxEmission, t);

      if (chargeTimer > chargeDuration) chargeTimer = 0;

      if (!ps.isPlaying)
        ps.Play();
    }
    else
    {
      chargeTimer = 0f;
      emission.rateOverTime = minEmission;
      if (ps.isPlaying)
        ps.Stop();
    }
  }

  public void StartCharging(float duration)
  {
    isCharging = true;
    chargeDuration = duration; // optionally adjust dynamically
  }

  public void StopCharging()
  {
    isCharging = false;
  }
}

