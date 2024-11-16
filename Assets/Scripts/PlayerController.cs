using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 1f;
  private Animator m_Animator;
  private Rigidbody2D m_rb;
  private SpriteRenderer m_SpriteRenderer;
  void Start()
  {
    m_rb = GetComponent<Rigidbody2D>();
    m_Animator = GetComponent<Animator>();
    m_SpriteRenderer = GetComponent<SpriteRenderer>();
    m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
  }
  void Update()
  {
    float horizontalInput = Input.GetAxis("Horizontal");

    m_rb.linearVelocity = new UnityEngine.Vector2(horizontalInput * moveSpeed, m_rb.linearVelocity.y);
    
    m_Animator.SetFloat("Speed", Mathf.Abs(m_rb.linearVelocityX));
    
    if (horizontalInput != 0)
    {
        m_SpriteRenderer.flipX = horizontalInput < 0;
    }
  }
}
