using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
        playerController.KillPlayer();
    }
}
