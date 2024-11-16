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
  void Start()
  {
    m_rb = GetComponent<Rigidbody2D>();
    m_Animator = GetComponent<Animator>();
    m_SpriteRenderer = GetComponent<SpriteRenderer>();
    m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    // Store original collider size and offset
    originalColliderSize = playerCollider.bounds.size;
    originalColliderOffset = playerCollider.offset;
  }
  void Update()
  {
    float horizontalInput = Input.GetAxis("Horizontal");
    float jumpInput = Input.GetAxis("Vertical");

    m_rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, m_rb.linearVelocity.y);
    
    m_Animator.SetFloat("Speed", Mathf.Abs(m_rb.linearVelocityX));
    
    if (horizontalInput != 0)
    {
        m_SpriteRenderer.flipX = horizontalInput < 0;
    }
    // Jumping
    if (jumpInput > 0 && IsGrounded())
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
  bool IsGrounded()
  {
    // Perform a simple ground check (adjust logic as necessary)
    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f);
    return hit.collider != null;
  }

}
