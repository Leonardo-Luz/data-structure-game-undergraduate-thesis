using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerMove : MonoBehaviour
{
  [Header("Movement Settings")]
  [SerializeField] private float speed = 3f;
  [SerializeField] private float moveSpeed = 3f;
  [SerializeField] private float runSpeed = 5f;
  [SerializeField] private float jumpForce = 6f;

  [Header("Dependencies")]
  [SerializeField] private Grounded groundedCheck;
  [SerializeField] private DustParticle dustParticle;

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
  }

  void Update()
  {
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    animator.SetBool("isCrouching", verticalInput < 0);

    isGrounded = groundedCheck.IsGrounded();

    dustParticle.UpdateWalkingDust(rb.linearVelocity.x, speed + 0.001 >= runSpeed, isGrounded);

    // Flip logic
    if (horizontalInput > 0 && spriteRenderer.flipX)
    {
      Flip(false);
    }
    else if (horizontalInput < 0 && !spriteRenderer.flipX)
    {
      Flip(true);
    }

    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
        speed = runSpeed;
    }

    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
        speed = moveSpeed;
    }

    // Move
    rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

    // Jump
    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      PlayerJump();
      dustParticle.PlayDust(!spriteRenderer.flipX);
    }

    // Cut jump short
    if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
    {
      rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }
  }

  void Flip(bool flipX)
  {
    spriteRenderer.flipX = flipX;
    dustParticle.TryPlayFlipDust(!flipX, isGrounded);
  }

  void PlayerJump()
  {
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
  }
}
