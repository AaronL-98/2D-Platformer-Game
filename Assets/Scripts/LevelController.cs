using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{   
    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_LevelEnd;
    [SerializeField] private GameObject m_LevelStart;
    
    void Awake()
    {
        if (m_LevelStart == null) m_LevelStart = GameObject.FindGameObjectWithTag("Restart");
        if (m_LevelEnd == null) m_LevelEnd = GameObject.FindGameObjectWithTag("Finish");
        if (m_Player == null) m_Player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Start()
    {
        ResetPlayerPosition();
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void ResetPlayerPosition()
    {
        if (m_LevelStart != null && m_Player != null) 
        {
            m_Player.transform.position = m_LevelStart.transform.position;
        } else {
            Debug.LogWarning("Player or LevelStart object not found!");
        }
    }
}
