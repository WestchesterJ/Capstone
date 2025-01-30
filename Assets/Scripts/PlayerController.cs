using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Gravity Settings")]
    public float fallMultiplier = 2.5f; // Multiplier for faster falling
    public float lowJumpMultiplier = 2f; // Multiplier for lower jumps if the button is released early

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Jump logic
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        ApplyGravityModifiers();
    }

    private void ApplyGravityModifiers()
    {
        // Apply a stronger gravity when falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Apply a softer gravity if the player releases the jump button early
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 size = GetComponent<Collider2D>().bounds.size;
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(position, size, 0f, Vector2.down, extraHeight, groundLayer);

        return hit.collider != null;
    }
}
