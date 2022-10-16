using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public AudioClip CoinSound;
    public AudioSource mAudioSource;
    public GameObject Player;

    void OnTriggerEnter(Collider other)
    {
        if (Player.gameObject.tag.Equals("Player")&& other.gameObject.tag.Equals("Coin"))
        {
            mAudioSource.PlayOneShot(CoinSound);
            ScoringSystem.score += 50;
            Destroy(other.gameObject);
        }
    }
}
