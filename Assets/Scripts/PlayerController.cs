using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
  public ScoreController scoreController;
  public float moveSpeed = 1f;
  public float jumpForce = 5f;
  private Animator m_Animator;
  private Rigidbody2D m_rb;
  private SpriteRenderer m_SpriteRenderer;
  private Collider2D playerCollider;


  public Vector2 crouchColliderSize = new Vector2(1f, 0.5f);
  public Vector2 crouchColliderOffset = new Vector2(0f, -0.25f);
  private Vector2 originalColliderSize, originalColliderOffset;
  private bool isCrouching = false;
  // Ground Check
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
  }
  void Update()
  {
    float horizontalInput = Input.GetAxis("Horizontal");
    // Move the player
    m_rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, m_rb.linearVelocity.y);
    // Flip the sprite based on direction
    if (horizontalInput != 0){
      m_SpriteRenderer.flipX = horizontalInput < 0;
    }
    // Set animator speed parameter
    m_Animator.SetFloat("Speed", Mathf.Abs(m_rb.linearVelocity.x));
    // Jumping
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
      Jump();
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
    // Check if grounded
    m_Animator.SetBool("Grounded", isGrounded);
    m_Animator.SetFloat("VerticalVelocity", m_rb.linearVelocity.y);
  }
  void Jump() 
  {
    // Apply jump force
    m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, jumpForce);

    // Trigger jump animation
    m_Animator.SetTrigger("Jump");

    // Set grounded state to false
    isGrounded = false;
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
  void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.CompareTag("Platform")) {
      isGrounded = true;
    }
  }

  void OnCollisionExit2D(Collision2D collision) {
    if (collision.gameObject.CompareTag("Platform")) {
      isGrounded = false;
    }
  }

    public void PickupKey()
    {
      Debug.Log("player picked up key");
      scoreController.IncrementScore(1);
    }
}
