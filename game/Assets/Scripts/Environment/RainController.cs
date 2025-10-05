using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RainController : MonoBehaviour
{
  [SerializeField] private float forwardTime = 1.0f;
  [SerializeField] private Transform followTarget;
  [SerializeField] private Vector3 offset = new Vector3(0, 11f, 0);

  private ParticleSystem ps;

  void Start()
  {
    ps = GetComponent<ParticleSystem>();
    followTarget = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

    // FIX: Simulate is used to foward the particle system start time, not working...
    // ps.Simulate(forwardTime, true, true, true);
    ps.Play(false);
  }

  void LateUpdate()
  {
    if (followTarget != null)
    {
      ParticleSystem.ShapeModule shape = ps.shape;
      shape.position = new Vector3(followTarget.position.x + offset.x, followTarget.position.y + offset.y, 0);
    }
  }

  private void StopRain()
  {
    ps.Stop(false);
  }

  private void StartRain()
  {
    ps.Play(false);
  }
}
