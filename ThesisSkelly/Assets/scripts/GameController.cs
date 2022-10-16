using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer.instance.BeginTimer();
    }
	private void OnCollisionEnter(Collision other)
	{
        if (Player.gameObject.tag.Equals("Player") && other.gameObject.tag.Equals("Enemy"))
        {
           Timer.instance.EndTimer();
        }
    }
}
