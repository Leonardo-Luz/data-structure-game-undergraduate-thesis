using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerMove : MonoBehaviour
{
  [Header("Movement Settings")]
  [SerializeField] private float speed = 3f;
  [SerializeField] private float crouchSpeed = 1.0f;
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

  /// NOTE: Rebound Timeout
  private float reboundTimer = 2f;
  private float reboundTimeout = 2f;

  private void Start()
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

  private void Update()
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

    // INFO: Particles logic
    dustParticle.UpdateWalkingDust(rb.linearVelocity.x, speed + 0.001 >= runSpeed, isGrounded);
    dustParticle.setScaleFactor(0.2f);
    dustParticle.TryPlayWalkDust(!spriteRenderer.flipX, isGrounded, Random.Range(0.6f, 1f));

    if (horizontalInput > 0 && spriteRenderer.flipX) Flip(false);
    else if (horizontalInput < 0 && !spriteRenderer.flipX) Flip(true);

    // INFO: Reset auxiliar variable to default
    float auxSpeed = speed;

    // INFO: Changes speeds based on current player state
    if (isCrouching) auxSpeed = crouchSpeed;
    else if (Input.GetKey(KeyCode.LeftShift)) auxSpeed = runSpeed;
    else auxSpeed = moveSpeed;

    // INFO: Locks player speed if jumping

    reboundTimer += Time.deltaTime;
    if (isGrounded)
    {
      speed = auxSpeed;

      if (reboundTimer >= reboundTimeout)
      {
        playerCombat.SetupRebound();
        reboundTimer = 0;
      }
    }

    // INFO: Locks player move when casting or knocked back
    if (!knockback.IsKnockedBack && !playerCombat.isCasting)
      rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

    // INFO: Stops player movement when casting
    if (playerCombat.isCasting) rb.linearVelocity = Vector2.zero;
    // INFO: Applies friction to player when casting
    //   rb.sharedMaterial.friction = 0.8f;
    // else rb.sharedMaterial.friction = 0f;

    if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching && !playerCombat.isCasting)
    {
      Jump();
      dustParticle.setScaleFactor(1f);
      dustParticle.PlayDust(!spriteRenderer.flipX);
    }

    bool isMidAir = rb.linearVelocity.y > 0;
    if (Input.GetButtonUp("Jump") && isMidAir) CancelJump();
  }

  private void Flip(bool flipX)
  {
    spriteRenderer.flipX = flipX;
    dustParticle.setScaleFactor(0.8f);
    dustParticle.TryPlayFlipDust(!flipX, isGrounded);

    playerCombat.FlipShootPoint(flipX);
  }

  private void Jump()
  {
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
  }

  private void CancelJump()
  {
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
  }
}
