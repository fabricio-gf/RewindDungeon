using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorSyncTest : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		GameManager.GM.Load("T2");
	}

}
