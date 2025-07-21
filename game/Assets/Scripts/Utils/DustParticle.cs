using UnityEngine;

public class DustParticle : MonoBehaviour
{
  [SerializeField] private ParticleSystem dust;
  [SerializeField] private float dustRunTimeThreshold = 1.5f;
  [SerializeField] private float minWalkSpeed = 0.1f;
  [SerializeField] private float scaleFactor = 1f;

  private float defaultSize = 0f;
  private float walkTimer = 0f;
  private float stopTimer = 0f;
  private bool canPlayDust = false;
  private float timeout = 0f;

  public void Start()
  {
    if (dust == null)
      dust = GetComponent<ParticleSystem>();

    var main = dust.main;
    var size = main.startSize;
    defaultSize = size.constant;
    size.constant *= scaleFactor;
    main.startSize = size;
  }

  public void UpdateWalkingDust(float horizontalVelocity, bool isRunning, bool isGrounded)
  {
    bool isMoving = Mathf.Abs(horizontalVelocity) > minWalkSpeed;

    if (timeout > 0)
    {
      timeout -= Time.deltaTime;
    }

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

  public void TryPlayWalkDust(bool facingRight, bool isGrounded, float timeout)
  {
    if (!canPlayDust || dust == null || this.timeout > 0) return;

    if (isGrounded)
      PlayDust(facingRight);

    this.timeout = timeout;
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


  public void setScaleFactor(float scaleFactor)
  {
    this.scaleFactor = scaleFactor;

    var main = dust.main;
    var size = main.startSize;
    size.constant = defaultSize * scaleFactor;
    main.startSize = size;
  }
}
