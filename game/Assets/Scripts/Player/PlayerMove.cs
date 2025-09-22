using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerMove : MonoBehaviour
{
  [Header("Movement Settings")]
  [SerializeField] private float speed = 3f;
  [SerializeField] private float crouchSpeed = 1.8f;
  [SerializeField] private float moveSpeed = 3f;
  [SerializeField] private float runSpeed = 5f;
  [SerializeField] private float jumpForce = 6f;

  [Header("Dependencies")]
  [SerializeField] private Grounded groundedCheck;
  [SerializeField] private DustParticle dustParticle;
  private Health health;
  private PlayerCollisions playerCollisions;
  private Knockback knockback;
  private PlayerCombat playerCombat;

  private Rigidbody2D rb;
  private SpriteRenderer spriteRenderer;

  [SerializeField] private Animator animator;

  private bool isGrounded;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    groundedCheck = GetComponent<Grounded>();
    dustParticle = GetComponent<DustParticle>();
    animator = GetComponent<Animator>();
    health = GetComponent<Health>();
    knockback = GetComponent<Knockback>();
    playerCollisions = GetComponent<PlayerCollisions>();
    playerCombat = GetComponent<PlayerCombat>();

    playerCollisions.OnEnemyHit += health.TakeDamage;
  }

  void Update()
  {
    float horizontalInput = 0;
    float verticalInput = 0;

    if (!knockback.IsKnockedBack && !playerCombat.isCasting)
    {
      horizontalInput = Input.GetAxis("Horizontal");
      verticalInput = Input.GetAxis("Vertical");
    }

    bool isCrouching = verticalInput < 0 && isGrounded;

    animator.SetBool("isCrouching", isCrouching);

    isGrounded = groundedCheck.IsGrounded();

    dustParticle.UpdateWalkingDust(rb.linearVelocity.x, speed + 0.001 >= runSpeed, isGrounded);
    dustParticle.setScaleFactor(0.2f);
    dustParticle.TryPlayWalkDust(!spriteRenderer.flipX, isGrounded, Random.Range(0.6f, 1f));

    if (horizontalInput > 0 && spriteRenderer.flipX)
    {
      Flip(false);
    }
    else if (horizontalInput < 0 && !spriteRenderer.flipX)
    {
      Flip(true);
    }

    float curSpeed = speed;

    if (isCrouching)
    {
      curSpeed = crouchSpeed;
    }
    else if (Input.GetKey(KeyCode.LeftShift))
    {
      curSpeed = runSpeed;
    }
    else
    {
      curSpeed = moveSpeed;
    }

    if (isGrounded)
      speed = curSpeed;

    if (!knockback.IsKnockedBack && !playerCombat.isCasting)
      rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

    if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching && !playerCombat.isCasting)
    {
      PlayerJump();
      dustParticle.setScaleFactor(1f);
      dustParticle.PlayDust(!spriteRenderer.flipX);
    }

    if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
    {
      rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }
  }

  void Flip(bool flipX)
  {
    spriteRenderer.flipX = flipX;
    dustParticle.setScaleFactor(0.8f);
    dustParticle.TryPlayFlipDust(!flipX, isGrounded);

    playerCombat.flipShootPoint(flipX);
  }

  void PlayerJump()
  {
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
  }
}
