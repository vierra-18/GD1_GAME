using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoringSystem : MonoBehaviour
{
	public GameObject scoreText;
	public static int score = 0;

	void Update()
	{
		scoreText.GetComponent<Text>().text = "Score: " + score;
		if(score == 400)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
