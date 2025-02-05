using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 7f; // Half the size of the regular jump
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool doubleJumpAvailable; // Tracks if the player can double jump
    private string horizontalAxis;
    private string jumpButton;

    private string basicAttackButton;

    [Header("Gravity Settings")]
    public float fallMultiplier = 2.5f; // Multiplier for faster falling
    public float lowJumpMultiplier = 2f; // Multiplier for lower jumps if the button is released early



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Determine input settings based on the player name
        if (gameObject.name == "Player")
        {
            horizontalAxis = "Horizontal";
            jumpButton = "Jump";
        }
        else if (gameObject.name == "Player 2")
        {
            horizontalAxis = "Horizontal 2";
            jumpButton = "Jump 2";
        }

        // Initialize double jump availability
        doubleJumpAvailable = false;
    }

    void Update()
    {
        // Handle horizontal movement
        float horizontalInput = Input.GetAxisRaw(horizontalAxis); // Gets the button
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Jump logic
        if (Input.GetButtonDown(jumpButton))
        {
            if (IsGrounded())
            {
                // Regular jump when grounded
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                doubleJumpAvailable = true; // Allow double jump after landing
            }
            else if (doubleJumpAvailable)
            {
                // Double jump when in the air
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                doubleJumpAvailable = false; // Disable further double jumps
            }
        }

        ApplyGravityModifiers();
    }

    private void ApplyGravityModifiers()
    {
        // Apply stronger gravity when falling over time
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Apply a softer gravity if the player releases the jump button early
        else if (rb.velocity.y > 0 && !Input.GetButton(jumpButton))
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
        // Makes a hitbox to feel for the ground
        bool grounded = hit.collider != null;

        // Reset double jump availability when grounded
        if (grounded)
        {
            doubleJumpAvailable = true;
        }

        return grounded;
    }

    private void basicAttack()
    {

    }
}