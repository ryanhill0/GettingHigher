using System.Collections;
using System.Collections.Generic;
using UnityEngine;


  

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public LayerMask groundLayer; // Layer for ground/platforms
    public float groundCheckDistance = 0.2f; // Distance to check for ground
    public float climbSpeed = 5f; // Speed at which the player climbs

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isClimbing; // To check if the player is climbing
    private bool isNearWall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
{
    MovePlayer();
    HandleJump();
    HandleClimbing(); // Updated for climbing logic
    KeepPlayerInBounds();
}

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = new Vector2(movement.x, rb.velocity.y);
    }

    void HandleJump()
{
    // Check if the player is grounded (on the ground or touching side walls)
    isGrounded = IsGrounded() || IsTouchingWall();
        //Debug.Log(isGrounded);
    // Jump when grounded (either on the ground or touching a side wall)
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
        Jump();
    }
    
    // Jump off the wall if the player is climbing
    if (Input.GetKeyDown(KeyCode.Space) && isNearWall)
    {
        // Stop climbing and apply a jump force upwards
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isClimbing = false; // Stop climbing behavior when jumping
    }
}
bool IsGrounded()
{
    // Raycast downwards to check for ground
    return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
}

bool IsTouchingWall()
{
    float extraDistance = 0.5f;  // Adjust depending on player size
    Vector2 raycastStart = new Vector2(transform.position.x, transform.position.y - 0.5f);  // Start lower to detect better
    RaycastHit2D hitLeft = Physics2D.Raycast(raycastStart, Vector2.left, extraDistance, groundLayer);
    RaycastHit2D hitRight = Physics2D.Raycast(raycastStart, Vector2.right, extraDistance, groundLayer);

    // Return true if touching either the left or right wall
    return hitLeft.collider != null || hitRight.collider != null;
}
void Jump()
{
    // Apply vertical velocity for the jump (only when grounded)
    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    isGrounded = false; // Prevent double jumps (reset grounded state)
}

    void HandleClimbing()
{
    // Raycasts to check if the player is near a wall (left or right)
    RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.5f, groundLayer);
    RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, groundLayer);
    isNearWall = hitLeft.collider != null || hitRight.collider != null;
    
    // Check for climb input (up or down) and ensure player isn't grounded
    if (isNearWall && !isGrounded && Input.GetAxisRaw("Vertical") != 0)
    {
        isClimbing = true;
            
        // Apply climbing velocity based on vertical input (up or down)
        float verticalInput = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed); // Set vertical velocity for climbing
    }
    else
    {
        isClimbing = false; // Stop climbing if not pressing vertical input or not near wall
    }
}


    void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Platform"))
    {
        isGrounded = true;
    }
}

void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Platform"))
    {
        isGrounded = false;
    }
}

    void KeepPlayerInBounds()
    {
        Camera camera = Camera.main;
        Vector3 screenPos = camera.WorldToViewportPoint(transform.position);
        screenPos.x = Mathf.Clamp(screenPos.x, 0.05f, 0.95f); // Keep player within camera bounds
        transform.position = camera.ViewportToWorldPoint(screenPos);
    }
}




