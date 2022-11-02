using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class XTRA_BOXCOLLIDE : MonoBehaviour
{

    int coins;
    int distance;
    float time;
    string timeTxt;
    string coinTxt;
    string distanceTxt;
    string filename;
    bool isDone = true;


    private void OnTriggerEnter(Collider collision)
    {
        // once seeker has met the player
        if (collision.gameObject.tag == "Player")
        {
            // START OF LOGGING TO TEXT FILE
            // moved distance to on trigger so it doesnt check every frame
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Gameplay_Results/");
            distance = Pathfinding.n;
            distanceTxt = "Max distance: " + distance + "\r\n";

            coins = ScoringSystem.score;
            coinTxt = "You got " + coins + " coins collected \r\n";


            time = Timer.elaspedTime;
            timeTxt = "You got " + time + " time elapsed \r\n";

            string playResult = "GOT CAUGHT BY SEEKER";

            // gets the WriteDebugToFile component script to be able to create the log file
            gameObject.GetComponent<WriteDebugToFile>().CreateTextFile(distanceTxt, coinTxt, timeTxt, playResult);
            // END OF LOGGING TO TEXT FILE

            SceneManager.LoadScene("You Died"); 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}


//   public void onWin()
// {
//     if (ScoringSystem.score == 400)
//     {
//         Directory.CreateDirectory(Application.streamingAssetsPath + "/Gameplay_Results/");

//         coins = ScoringSystem.score;

//         coinTxt = "You got " + coins + " coins collected \n";

//         time = Timer.elaspedTime;

//         distanceTxt = "Max distance: " + distance + " \n";


//         timeTxt = "You got " + time + " time elapsed \n";
//         // CreateTextFile();

//     }
// }



// changed formatting
// void CreateTextFile()
// {
//     string txtDocumentName = Application.streamingAssetsPath + "/Gameplay_Results/" + "SampleFile" + ".txt";
//     if (!File.Exists(txtDocumentName))
//     {
//         File.WriteAllText(txtDocumentName, "GAME RESULTS" + "\r\n" /*+ distanceTxt + "\n\n"*/); // commented out unnecessary distance
//     }
//     File.AppendAllText(txtDocumentName,
//         "============================================ \r\n" +
//         distanceTxt +
//         coinTxt +
//         timeTxt + "\r\n"
//         );
// }

// private void OnCollisionEnter(Collision collision)
// {
//     if (collision.gameObject.tag == "Player")
//     {

//      Directory.CreateDirectory(Application.streamingAssetsPath + "/Gameplay_Results/");

// coins = ScoringSystem.score;

// coinTxt = "You got " + coins + " coins collected \r\n";

// time = Timer.elaspedTime;

// distanceTxt = "Max distance: " + distance + "\r\n";


// timeTxt = "You got " + time + " time elapsed \r\n";

// CreateTextFile();
//     }

// }

// public void Log(string logString, string stackTrace, LogType type)
// {
//     TextWriter tw = new StreamWriter(filename, true);
//     tw.WriteLine("" + logString);
//     tw.WriteLine("[" + System.DateTime.Now + "]" + logString);
//     tw.Close();
// }

// Start is called before the first frame update

// private void OnEnable()
// {
//     Application.logMessageReceived += Log;
// }