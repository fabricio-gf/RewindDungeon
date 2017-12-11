using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorSyncTest : MonoBehaviour {

	public GameObject actorPrefab;

	LevelManager lm;

	void Awake() {
		DontDestroyOnLoad(gameObject);
		lm = GameObject.FindObjectOfType<LevelManager>();
	}

	void Start() {
		lm.Load("T2");
		SceneManager.sceneLoaded += FinishedLoading;
	}

	void FinishedLoading(Scene scene, LoadSceneMode mode) {
		SceneManager.sceneLoaded -= FinishedLoading;
		StartCoroutine(WaitTillLoaded());
	}

	IEnumerator WaitTillLoaded() {
		while (lm.state != LevelManager.State.PLANNING) {
			yield return new WaitForSeconds(0.25f);
		}
		lm.playerSpawnPoints.ForEach(
			spawnPoint => spawnPoint.Spawn(lm, actorPrefab));
		InitPlayers();
	}

	void InitPlayers() {
		lm.actors[0].AddAction(Actor.Action.MOVE_R);
		lm.actors[0].AddAction(Actor.Action.MOVE_R);
		lm.actors[1].AddAction(Actor.Action.MOVE_U);
		lm.actors[1].AddAction(Actor.Action.MOVE_D);
		lm.actors.ForEach(
			actor => actor.BeginPlan());
	}
}
