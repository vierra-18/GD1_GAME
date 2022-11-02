using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Game exiting");
        Application.Quit();
    }

    public void LoadPathfindingTest()
    {
        SceneManager.LoadScene("Test Maze_Pathfinding2");
    }
    public void LoadTutorial() 
    {
        SceneManager.LoadScene("Tutorial");
    }
    

}
