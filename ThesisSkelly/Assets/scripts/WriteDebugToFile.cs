using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class WriteDebugToFile : MonoBehaviour
{   
    public void CreateTextFile(String distanceTxt, String coinTxt, String timeTxt, String playResult)
    {
        string txtDocumentName = Application.streamingAssetsPath + "/Gameplay_Results/" + "SampleFile" + ".txt";
        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "GAME RESULTS" + "\r\n" /*+ distanceTxt + "\n\n"*/); // commented out unnecessary distance
        }
        File.AppendAllText(txtDocumentName,
            "============================================ \r\n" +
            distanceTxt +
            coinTxt +
            timeTxt + 
			playResult +"\r\n"
            );
    }

}


// string filename = "";

    // private void OnEnable()
    // {
    // 	Application.logMessageReceived += Log;
    // }



    // private void OnDisable()
    // {
    // 	Application.logMessageReceived -= Log;
    // }
    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {
    // 	filename = Application.streamingAssetsPath + "Logfile.text";
    // }
    // public void Log(string logString, string stackTrace, LogType type)
    // {
    // 	TextWriter tw = new StreamWriter(filename, true);
    // 	tw.WriteLine(""+logString+"");

    // 	tw.WriteLine("[" + System.DateTime.Now + "]" + logString); 
    // 	tw.Close();
    // }