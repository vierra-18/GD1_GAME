using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class XTRA_BOXCOLLIDE : MonoBehaviour
{

    int coins;
    int distance;
    float time;
    string timeTxt;
    string coinTxt;
    string distanceTxt;
    string filename;



    // Start is called before the first frame update

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }
    void Start()
    {

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            print("here");

            Directory.CreateDirectory(Application.streamingAssetsPath + "/Gameplay_Results/");

            coins = ScoringSystem.score;

            coinTxt = "You got " + coins + " coins collected";

            time = Timer.elaspedTime;
            distanceTxt = "Max distance " + distance;
            CreateTextFile();

            timeTxt = "You got " + time + " time elapsed";

            CreateTextFile();
        }
        
    }

    void CreateTextFile()
    {
        string txtDocumentName = Application.streamingAssetsPath + "/Gameplay_Results/" + "SampleFile" + ".txt";
        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "SampleTitle" + "\n\n" + distanceTxt);
        }
        File.AppendAllText(txtDocumentName,

            coinTxt + "\n" +
            "\n"+ timeTxt
            );
    }
    public void Log(string logString, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(filename, true);
        tw.WriteLine("" + logString);
        tw.WriteLine("[" + System.DateTime.Now + "]" + logString);
        tw.Close();
    }


}
