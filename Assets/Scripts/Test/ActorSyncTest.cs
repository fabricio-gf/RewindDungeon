using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorSyncTest : MonoBehaviour {

	public GameObject actorPrefab;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		GameManager.LM.Load("T2");
		SceneManager.sceneLoaded += FinishedLoading;
	}

	void FinishedLoading(Scene scene, LoadSceneMode mode) {
		SceneManager.sceneLoaded -= FinishedLoading;
		StartCoroutine(WaitTillLoaded());
	}

	IEnumerator WaitTillLoaded() {
		while (GameManager.LM.state != LevelManager.State.PLANNING) {
			yield return new WaitForSeconds(0.25f);
		}
		GameManager.LM.playerSpawnPoints.ForEach(
			spawnPoint => spawnPoint.Spawn(actorPrefab));
	}
}
