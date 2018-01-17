using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public int CurrentScore;

	// Use this for initialization
	void Awake () {
        CurrentScore = 1;
	}
	
	public void AddCoin()
    {
        if(CurrentScore < 3)
        {
            CurrentScore++;
        }
    }
}
