using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialWinLose : MonoBehaviour
{
    // Start is called before the first frame update
   int coins;
    int distance;
    float time;
    string timeTxt;
    string coinTxt;
    string distanceTxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Win();
    }
	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Enemy") 
        {
            SceneManager.LoadScene("Main Menu");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
    void Win() 
    {
        if (ScoringSystem.score == 100)
         {
           
            SceneManager.LoadScene("Main Menu");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
   
}
