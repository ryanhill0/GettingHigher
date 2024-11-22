using System.Collections;
using System.Collections.Generic;
using UnityEngine;


  

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public LayerMask groundLayer; 
    public float groundCheckDistance = 0.2f; 
    public float climbSpeed = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isClimbing;
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
    HandleClimbing(); 
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
    
    isGrounded = IsGrounded() || IsTouchingWall();
        //Debug.Log(isGrounded);
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
        Jump();
    }
    
    
    if (Input.GetKeyDown(KeyCode.Space) && isNearWall)
    {
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isClimbing = false; 
    }
}
bool IsGrounded()
{
    
    return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
}

bool IsTouchingWall()
{
    float extraDistance = 0.5f;  // Adjust depending on player size
    Vector2 raycastStart = new Vector2(transform.position.x, transform.position.y - 0.5f);  // Start lower to detect better
    RaycastHit2D hitLeft = Physics2D.Raycast(raycastStart, Vector2.left, extraDistance, groundLayer);
    RaycastHit2D hitRight = Physics2D.Raycast(raycastStart, Vector2.right, extraDistance, groundLayer);

   
    return hitLeft.collider != null || hitRight.collider != null;
}
void Jump()
{
    
    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    isGrounded = false; 
}

    void HandleClimbing()
{
    RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.5f, groundLayer);
    RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, groundLayer);
    isNearWall = hitLeft.collider != null || hitRight.collider != null;
    
    if (isNearWall && !isGrounded && Input.GetAxisRaw("Vertical") != 0)
    {
        isClimbing = true;
            
        float verticalInput = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed); 
    }
    else
    {
        isClimbing = false; 
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
        screenPos.x = Mathf.Clamp(screenPos.x, 0.05f, 0.95f);
        transform.position = camera.ViewportToWorldPoint(screenPos);
    }
}




