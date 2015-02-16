using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
    public bool GameStarted;

	void Start () 
    {
        GameStarted = false;
	}

    public void StartGame()
    {
        GameStarted = true;
    }

	void Update () 
    {

	}
}
