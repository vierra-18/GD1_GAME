using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public static Timer instance;
    public Text timeCounter;

    private TimeSpan timePlaying;
    private bool timerGoing;

    public static float elaspedTime;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Start()
    {
        timeCounter.text = "Time: 00:00.00";
        timerGoing = false;
    }
    public void BeginTimer() 
    {
        timerGoing = true;
        elaspedTime = 0f;
        StartCoroutine(UpdateTimer());
    }
	private void Update()
	{
        print(timerGoing);
	}
	public void EndTimer() 
    {
        timerGoing= false;
    }

	private IEnumerator UpdateTimer()
	{
        while (timerGoing) 
        {
            elaspedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elaspedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;
            yield return null;
        }
	}
}
