using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Collection : MonoBehaviour
{
    int coins;
    int distance;
    float time;
    string timeTxt;
    string coinTxt;
    string distanceTxt;

    public AudioClip CoinSound;
    public AudioSource mAudioSource;
    public GameObject Player;

    void OnTriggerEnter(Collider other)
    {
        if (Player.gameObject.tag.Equals("Player") && other.gameObject.tag.Equals("Coin"))
        {
            mAudioSource.PlayOneShot(CoinSound);
            ScoringSystem.score += 50;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag.Equals("Coin") && ScoringSystem.score == 400)
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

            string playResult = "COLLECTED NEEDED COINS";

            // gets the WriteDebugToFile component script to be able to create the log file
            gameObject.GetComponent<WriteDebugToFile>().CreateTextFile(distanceTxt, coinTxt, timeTxt, playResult);
            // END OF LOGGING TO TEXT FILE
        }
    }
}
