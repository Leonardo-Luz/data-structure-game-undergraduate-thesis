using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RainController : MonoBehaviour
{
  [SerializeField] private Transform followTarget;
  [SerializeField] private Vector3 offset = new Vector3(0, 17f, 0);

  private ParticleSystem ps;

  void Start()
  {
    ps = GetComponent<ParticleSystem>();
    followTarget = Camera.main.transform;
    ps.Play();
  }

  void LateUpdate()
  {
    if (followTarget != null)
    {
      // Move emitter only horizontally to keep it near the camera
      Vector3 pos = followTarget.position + offset;
      pos.z = 0;
      transform.position = pos;
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
