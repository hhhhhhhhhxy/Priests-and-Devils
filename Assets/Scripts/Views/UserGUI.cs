using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    public string gameMessage;
    public int time;
    public bool isGameOver;

    public void SetGameOver(bool gameOver)
    {
        isGameOver = gameOver;
    }

    void OnGUI()
    {
        // Display game message
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), gameMessage);
        
        // Display time
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 25, 100, 50), "Time: " + time);

        // Display restart button if game is over
        if (isGameOver)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 75, 100, 50), "Restart"))
            {
                RestartGame();
            }
        }
    }

    void RestartGame()
    {
        // Reset game state
        gameMessage = "";
        time = 60;
        isGameOver = false;
        ((FirstController)SSDirector.GetInstance().CurrentSceneController).Restart();
    }
}