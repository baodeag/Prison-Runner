using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject); // Ensure only one GameSession exists
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Keep this GameSession across scenes
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString(); // Initialize lives text
        scoreText.text = playerScore.ToString(); // Initialize score text
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession(); // If no lives left, reset the game session
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        playerScore += pointsToAdd;
        scoreText.text = playerScore.ToString(); // Update score text
    }


    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex); // Reload the current scene
        livesText.text = playerLives.ToString(); // Initialize lives text
    }
    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist(); // Reset the ScenePersist object
        SceneManager.LoadScene(0);
        Destroy(gameObject); // Destroy the GameSession object to reset the game state

    }
}
