using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ResultsHold : MonoBehaviour
{
    //public TextMeshProUGUI levelText;
   // public TextMeshProUGUI timeText;
    //public TextMeshProUGUI coinText;
    //public TextMeshProUGUI deathText;

    int level;
    int time;
    int coins;
    int deaths;

    string fileName;
    string levelTxt;
    string timeTxt;
    string coinTxt;
    string deathTxt;

    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Gameplay_Results/");

       //level = PassIt.level;
        //time = PassIt.finalTime;
        coins = ScoringSystem.score;
        //deaths = PassIt.finalDeaths;

        //fileName = "Level " + level + " completed";
        //timeTxt = "Time: " + time + " seconds";
        coinTxt = "You got " + coins + " coins collected";
        //deathTxt = "Deaths: " + deaths + " times";

        //levelText.text = fileName;
        //timeText.text = timeTxt;
        //coinText.text = coinTxt;
        //deathText.text = deathTxt;

        CreateTextFile();
    }
    void CreateTextFile()
    {
        
        string txtDocumentName = Application.streamingAssetsPath + "/Gameplay_Results/" + "SampleFIle"+".txt";
        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "SamplleTitle" + "\n\n");
        }
        File.AppendAllText(txtDocumentName, 
              
            coinTxt + "\n" + 
            "\n");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
