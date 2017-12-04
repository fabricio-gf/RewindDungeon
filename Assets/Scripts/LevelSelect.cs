using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {
	
	void LoadLevels(){
		//loads levels and information like is it locked, number of coins, thumbnail(?)
	}

	public void ChangeScene(string str){
		SceneManager.LoadScene(str);
	}
}
