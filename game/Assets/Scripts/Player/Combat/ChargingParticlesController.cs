using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ChargeParticlesController : MonoBehaviour
{
  private ParticleSystem ps;
  private ParticleSystem.EmissionModule emission;
  private ParticleSystem.MainModule main;
  private ParticleSystem.VelocityOverLifetimeModule velocity;

  [Header("Particle Settings")]
  public float maxEmission = 30f;
  public float minEmission = 4f;
  public float chargeDuration = 1f;
  public Color chargingColor = Color.white;
  public Color castingColor = Color.cyan;

  private float chargeTimer = 0f;
  private bool isCharging = false;
  private bool isCasting = false;

  void Awake()
  {
    ps = GetComponent<ParticleSystem>();
    emission = ps.emission;
    main = ps.main;
    velocity = ps.velocityOverLifetime;
    velocity.enabled = true;
    ps.Stop();
  }

  void Update()
  {
    if (isCharging || isCasting)
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

  // --------------------------
  // Charging (inward)
  // --------------------------
  public void StartCharging(float duration)
  {
    isCharging = true;
    isCasting = false;
    chargeDuration = duration;
    main.startColor = chargingColor;
    SetParticlesDirection(true);
  }

  public void StopCharging()
  {
    isCharging = false;
  }

  // --------------------------
  // Casting (outward)
  // --------------------------
  public void StartCasting(float duration)
  {
    isCasting = true;
    isCharging = false;
    chargeDuration = duration;
    main.startColor = castingColor;
    SetParticlesDirection(false);
  }

  public void StopCasting()
  {
    isCasting = false;
  }

  // Helper: set particle velocity toward center (inward) or away (outward)
  private void SetParticlesDirection(bool inward)
  {
    float speed = 0.3f; // adjust as needed
    velocity.radial = inward ? new ParticleSystem.MinMaxCurve(-speed) : new ParticleSystem.MinMaxCurve(speed);
  }
}
