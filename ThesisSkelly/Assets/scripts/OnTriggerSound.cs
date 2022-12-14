using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerSound : MonoBehaviour
{
    public AudioClip Scream;
    public AudioSource mAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag.Equals("Player")) 
        {
            StartCoroutine(scream());

        }
	}
    IEnumerator scream()
    {
        mAudioSource.Play();
        yield return new WaitWhile(() => mAudioSource.isPlaying);
        //do something
    }

}
