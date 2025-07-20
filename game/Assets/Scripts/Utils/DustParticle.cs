using UnityEngine;

public class DustParticle : MonoBehaviour
{
  private ParticleSystem dust;
  [SerializeField] private float dustRunTimeThreshold = 1.5f;
  [SerializeField] private float minWalkSpeed = 0.1f;

  private float walkTimer = 0f;
  private float stopTimer = 0f;
  private bool canPlayDust = false;

  public void Start()
  {
    dust = gameObject.GetComponent<ParticleSystem>();
  }

  public void UpdateWalkingDust(float horizontalVelocity, bool isRunning, bool isGrounded)
  {
    bool isMoving = Mathf.Abs(horizontalVelocity) > minWalkSpeed;

    if (isMoving && isGrounded && isRunning)
    {
      walkTimer += Time.deltaTime;
      stopTimer = 0f;

      if (walkTimer >= dustRunTimeThreshold)
      {
        canPlayDust = true;
      }
    }
    else
    {
      stopTimer += Time.deltaTime;
    }

    if (stopTimer >= 0.2f)
    {
      walkTimer = 0f;
      canPlayDust = false;
    }
  }

  public void TryPlayFlipDust(bool facingRight, bool isGrounded)
  {
    if (!canPlayDust || dust == null)
    {
      walkTimer = 0f;
      return;
    }

    if (isGrounded)
      PlayDust(facingRight);

    canPlayDust = false;
    walkTimer = 0f;
  }

  public void PlayDust(bool facingRight)
  {
    var shape = dust.shape;
    Vector3 scale = shape.scale;
    Vector3 pos = shape.position;

    scale.x = Mathf.Abs(scale.x) * (facingRight ? -1 : 1);
    pos.x = Mathf.Abs(pos.x) * (facingRight ? -1 : 1);

    shape.scale = scale;
    shape.position = pos;

    dust.Play();
  }
}
