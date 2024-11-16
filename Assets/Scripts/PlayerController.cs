using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 1f;
  public float jumpForce = 5f;
  private Animator m_Animator;
  private Rigidbody2D m_rb;
  private SpriteRenderer m_SpriteRenderer;
  private Collider2D playerCollider;

  public Vector2 crouchColliderSize = new Vector2(1f, 0.5f);
  public Vector2 crouchColliderOffset = new Vector2(0f, -0.25f);
  private Vector2 originalColliderSize;
  private Vector2 originalColliderOffset;
  private bool isCrouching = false;
  // Ground Check
  public Transform groundCheck; // Empty GameObject positioned at the player's feet
  public float groundCheckRadius = 0.2f; // Radius of the overlap circle
  public LayerMask groundLayer; // Layer assigned to ground objects
  private bool isGrounded;
  void Start()
  {
    m_rb = GetComponent<Rigidbody2D>();
    m_Animator = GetComponent<Animator>();
    m_SpriteRenderer = GetComponent<SpriteRenderer>();
    playerCollider = GetComponent<Collider2D>();
    
    m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    // Store original collider size and offset
    originalColliderSize = playerCollider.bounds.size;
    originalColliderOffset = playerCollider.offset;

    // Automatically create the GroundCheck object if not assigned
    if (groundCheck == null)
    {
      GameObject groundCheckObject = new GameObject("GroundCheck");
      groundCheckObject.transform.parent = transform; // Make it a child of the player
      groundCheckObject.transform.localPosition = new Vector3(0, -1f, 0); // Position it at the player's feet
      groundCheck = groundCheckObject.transform; // Assign the transform
    }
  }
  void Update()
  {
    float horizontalInput = Input.GetAxis("Horizontal");

    m_rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, m_rb.linearVelocity.y);
    
    m_Animator.SetFloat("Speed", Mathf.Abs(m_rb.linearVelocityX));
    
    if (horizontalInput != 0)
    {
        m_SpriteRenderer.flipX = horizontalInput < 0;
    }

    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    m_Animator.SetBool("Grounded", isGrounded);

    // Jumping
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
      m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, jumpForce);
      m_Animator.SetTrigger("Jump");
    }
    // Crouching
    if (Input.GetKey(KeyCode.LeftControl))
    {
      Crouch();
    }
    else if (isCrouching)
    {
      StandUp();
    }

    Debug.Log("grounded: " + m_Animator.GetBool("Grounded") + " crouch: " + m_Animator.GetBool("Crouch"));
  }
  void Crouch()
  {
    if (isCrouching) return;

    isCrouching = true;
    m_Animator.SetBool("Crouch", true);

    // Resize the collider for crouching
    var boxCollider = playerCollider as BoxCollider2D;
    if (boxCollider != null)
    {
        boxCollider.size = crouchColliderSize;
        boxCollider.offset = crouchColliderOffset;
    }
  }
  void StandUp()
  {
    isCrouching = false;
    m_Animator.SetBool("Crouch", false);

    // Reset the collider size and offset
    var boxCollider = playerCollider as BoxCollider2D;
    if (boxCollider != null)
    {
        boxCollider.size = originalColliderSize;
        boxCollider.offset = originalColliderOffset;
    }
  }
  void OnDrawGizmosSelected()
  {
    // Draw the ground check radius in the editor for debugging
    if (groundCheck != null)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
  }
}
