using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numScenePersists = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersists > 1)
        {
            Destroy(gameObject); // Ensure only one GameSession exists
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Keep this GameSession across scenes
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject); // Destroy the ScenePersist object to reset the game state
    }
}
