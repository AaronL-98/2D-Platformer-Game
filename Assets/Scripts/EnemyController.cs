using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speed = 2f;
    public Transform startPoint;       // Starting point of the patrol
    public Transform endPoint;         // Ending point of the patrol
    private bool movingRight = true;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Patrol();
    }

        private void Patrol()
    {
        // Move the enemy in the current direction
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);

        // Check if the enemy has reached the start or end point
        if (movingRight && transform.position.x >= endPoint.position.x)
        {
            Flip();
        }
        else if (!movingRight && transform.position.x <= startPoint.position.x)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Flip the movement direction
        movingRight = !movingRight;
        // Flip the sprite
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
        playerController.KillPlayer();
    }

    void OnDrawGizmos() 
    {
        // Visualize the patrol path in the editor
        Gizmos.color = Color.blue;
        if (startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(startPoint.position, endPoint.position);
        }
    }
}
