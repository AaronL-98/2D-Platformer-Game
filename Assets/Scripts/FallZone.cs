using UnityEngine;

public class FallZone : MonoBehaviour
{
    private LevelController levelController;
    void Start() 
    {
        // Find the LevelManager in the scene
        GameObject levelManager = GameObject.Find("LevelManager");

        if (levelManager != null) 
        {
            levelController = levelManager.GetComponent<LevelController>();
        }

        if (levelManager == null) 
        {
            Debug.LogError("LevelManager not found in the scene!");
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Check if the object entering the FallZone is the player
        if (other.CompareTag("Player")) 
        {
            // Call ResetPosition on the LevelController
            levelController?.ResetPlayerPosition();
        }
    }
}
