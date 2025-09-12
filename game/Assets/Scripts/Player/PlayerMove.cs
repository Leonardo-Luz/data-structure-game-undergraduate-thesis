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

  [Header("Accessibility Settings")]
  [SerializeField] private bool glow = false;

  [Header("Dependencies")]
  [SerializeField] private Grounded groundedCheck;
  [SerializeField] private DustParticle dustParticle;
  private GameObject glowCrouch;
  private GameObject glowIdle;

  private Rigidbody2D rb;
  private SpriteRenderer spriteRenderer;

  [SerializeField] private Animator animator;

  private bool isGrounded;

  void Start()
  {
    glowCrouch = GameObject.FindGameObjectWithTag("GlowCrouch");
    glowIdle = GameObject.FindGameObjectWithTag("GlowIdle");

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

    bool isCrouching = verticalInput < 0 && isGrounded;

    animator.SetBool("isCrouching", isCrouching);

    isGrounded = groundedCheck.IsGrounded();

    if(glow){
      glowCrouch.SetActive(isCrouching);
      glowIdle.SetActive(!isCrouching);
    } else {
      glowCrouch.SetActive(false);
      glowIdle.SetActive(false);
    }

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

    if(isGrounded)
      speed = curSpeed;

    rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

    if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
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
    SpriteRenderer glowIS = glowIdle.GetComponent<SpriteRenderer>();
    SpriteRenderer glowCS = glowCrouch.GetComponent<SpriteRenderer>();

    glowIS.transform.position = new Vector3(
        this.transform.position.x + ( flipX ? -0.031f : 0.031f ),
        this.transform.position.y - 0.0315f,
        this.transform.position.z
    );

    glowCS.transform.position = new Vector3(
        this.transform.position.x + ( flipX ? -0.000f : 0.000f ),
        this.transform.position.y - 0.01f,
        this.transform.position.z
    );

    glowIS.flipX = flipX;
    glowCS.flipX = flipX;

    spriteRenderer.flipX = flipX;
    dustParticle.setScaleFactor(0.8f);
    dustParticle.TryPlayFlipDust(!flipX, isGrounded);
  }

  void PlayerJump()
  {
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
  }
}
