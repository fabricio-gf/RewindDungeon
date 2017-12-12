using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class GameManager : MonoBehaviour {
	public static GameManager GM {
		get; private set;
	}
	public static LevelManager LM {
		get; private set;
	}

	void Awake() {
		if (GameObject.FindGameObjectsWithTag("GM").Length > 1) {
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			GM = GetComponent<GameManager>();
			LM = GetComponent<LevelManager>();
		}
	}

}
