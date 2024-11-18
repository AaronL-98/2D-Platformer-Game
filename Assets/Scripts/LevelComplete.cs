using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletePanel;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.GetComponent<PlayerController>() != null)
        {
            // Level is Over
            Debug.Log("Level finished by the player");
            levelCompletePanel.SetActive(true);
        }
    }
}
