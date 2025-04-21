using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private SpriteRenderer playerSprite;

    private Rigidbody2D rb;

    private bool isGrounded;

    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0 && playerSprite.flipX)
        {
            flip();
        }
        else if (horizontalInput < 0 && !playerSprite.flipX)
        {
            flip();
        }

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            PlayerJump();
        }
    }

    void flip()
    {
        playerSprite.flipX = !playerSprite.flipX;
    }

    void PlayerJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}
