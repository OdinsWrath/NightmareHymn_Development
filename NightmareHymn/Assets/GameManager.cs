﻿using UnityEngine;
using UnityEngine.SceneManagement;  // Provides alteration of scenes and game state

/*
 * This object/script is responsible for maintaining the game
 * rules and status. All changes in game state and scene/menu
 * loading must be done here.
 */

public class GameManager : MonoBehaviour
{
    bool isGameOver = false;
    public float restartDelay = 1f;  // Value representing how long to delay restarting the level

    public void WinLevel()
    {
        Debug.Log("WIN LEVEL!");
        if (SceneManager.GetActiveScene().name == "JaredLv1_Alpha")
        {
            SceneManager.LoadScene("Ryanlvl2");
        }
        else
        {
            SceneManager.LoadScene("StartMenu");
        }
        
    }

    // Defines the behavior in the event the game has reached the game over state
    public void EndGame()
    {
        if (isGameOver == false)
        {
            isGameOver = true;
            Debug.Log("Game Over");
            Invoke("RestartLevel", restartDelay);
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
