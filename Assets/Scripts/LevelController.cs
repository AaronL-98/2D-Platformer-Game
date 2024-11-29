using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{   
    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_LevelUI;
    [SerializeField] private GameObject m_LevelEnd;
    [SerializeField] private GameObject m_LevelStart;
    [SerializeField] private int m_LevelLives = 3;
    private int m_currentLives;
    private Image[] hearts; // Assign heart Image objects in the Inspector
    
    void Start()
    {
        if (m_LevelStart == null) m_LevelStart = GameObject.FindGameObjectWithTag("Restart");
        if (m_LevelEnd == null) m_LevelEnd = GameObject.FindGameObjectWithTag("Finish");
        if (m_Player == null) m_Player = GameObject.FindGameObjectWithTag("Player");
        if (m_LevelUI == null) m_LevelUI = GameObject.Find("panel_levelUI");
                    
        // Get the HeartDisplay panel from panel_levelUI
        Transform heartDisplay = m_LevelUI.transform.Find("HeartDisplay");
        if (heartDisplay != null) // Dynamically get the heart images from the panel_levelUI
        {
            hearts = heartDisplay.GetComponentsInChildren<Image>();
        } 
        else 
        {
            Debug.LogError("HeartDisplay not found in panel_levelUI!");
        }

        ResetPlayerPosition();
        m_currentLives = m_LevelLives;
        m_LevelUI.SetActive(true);
        UpdateHeartDisplay();
    }

    public void LoseLife()
    {
        m_currentLives--;
        if (m_currentLives > 0)
        {
            // Reset player to the starting position
            ResetPlayerPosition();
            UpdateHeartDisplay();
        } else 
        {
            // Restart the level after all lives are lost
            ReloadLevel();
        }
    }
    
    public void ReloadLevel()
    {
        Debug.Log("Restarting Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ResetPlayerPosition()
    {
        if (m_LevelStart != null && m_Player != null) 
        {
            m_Player.transform.position = m_LevelStart.transform.position;
        } else {
            Debug.LogWarning("Player or LevelStart object not found!");
        }

        // Reset player velocity to zero (if Rigidbody2D is attached)
        Rigidbody2D rb = m_Player.GetComponent<Rigidbody2D>();
        if (rb != null) 
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    private void UpdateHeartDisplay()
    {
        // Update hearts based on current lives
        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < m_currentLives)
            {
                hearts[i].enabled = true; // show hearts
            }
            else
            {
                hearts[i].enabled = false; // hide hearts
            }
        }
    }
}
