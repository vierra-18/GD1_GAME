using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class WinORLose : MonoBehaviour
{
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
            SceneManager.LoadScene("You Died");
        }

    }
    void Win() 
    {
        if (ScoringSystem.score == 500)
         {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Gameplay_Results/");

            coins = ScoringSystem.score;

            coinTxt = "You got " + coins + " coins collected \n";

            time = Timer.elaspedTime;

            distanceTxt = "Max distance: " + distance + " \n";


            timeTxt = "You got " + time + " time elapsed \n";
            CreateTextFile();
            SceneManager.LoadScene("You Win");

            Cursor.lockState = CursorLockMode.None;
        }
    }
    void CreateTextFile()
 {
    string txtDocumentName = Application.streamingAssetsPath + "/Gameplay_Results/" + "SampleFile" + ".txt";
    if (!File.Exists(txtDocumentName))
   {
        File.WriteAllText(txtDocumentName, "GAME RESULTS" + "\r\n" + distanceTxt + "\n\n"); // commented out unnecessary distance
   }
         File.AppendAllText(txtDocumentName,
         "============================================ \r\n" +
         distanceTxt +
        coinTxt +
         timeTxt + "\r\n"
         );
 }
}
