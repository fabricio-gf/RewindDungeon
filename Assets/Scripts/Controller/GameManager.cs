using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameManager))]
public class GameManager : MonoBehaviour {
	
	public enum State {
		OUT_OF_LEVEL,
		LOADING,
		PLANNING,
		RUNNING
	}

	public static GameManager GM {
		get; private set;
	}
	
	public float stepLoopDelay = 0.1f;

	public GameObject prefabWall;
	public GameObject prefabWarrior;
    public GameObject prefabArcher;

    public GameObject CharacterToSpawn;

	public GameObject prefabSpawnPoint;

	public List<Actor> actors;
	public List<GameObject> playerAvailableCharactersPrefabs;
	public List<PlayerSpawnPoint> playerSpawnPoints;

	public string levelToLoad;
	public string levelName;
	public int timeLimit;

	public Board board;
	public State state;

	public Actor selectedActor;

	void Awake() {
        if (GameObject.FindGameObjectsWithTag("GM").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            GM = GetComponent<GameManager>();
        }

		state = State.OUT_OF_LEVEL;
		selectedActor = null;
	}

	public void Init() {
		state = State.PLANNING;
		actors = new List<Actor>();
	}

	public void ExitLevel() {
		state = State.OUT_OF_LEVEL;
		// TODO return to level select
	}
/*
	void SceneLoaded(Scene scene, LoadSceneMode mode) {
		SceneManager.sceneLoaded -= SceneLoaded;

		board = GameObject.FindObjectOfType<Board>();
		TextAsset ta = Resources.Load(
			"Levels/" + levelToLoad,
			typeof(TextAsset)) as TextAsset;
		state = State.LOADING;

		using (StringReader reader = new StringReader(ta.text)) {
			levelName = reader.ReadLine();
			timeLimit = Convert.ToInt32(reader.ReadLine());
			playerSpawnPoints = new List<PlayerSpawnPoint>();
			int numWalls = Convert.ToInt32(reader.ReadLine());
			int numPlayerC = Convert.ToInt32(reader.ReadLine());
			int numEnemy = Convert.ToInt32(reader.ReadLine());
			int numHazard = Convert.ToInt32(reader.ReadLine());

			// walls
			for (int i = 0; i < numWalls; i++) {
				string[] wallPosStr = reader.ReadLine().Split(
						default(char[]),
						StringSplitOptions.RemoveEmptyEntries);
				int r = Convert.ToInt32(wallPosStr[0]);
				int c = Convert.ToInt32(wallPosStr[1]);

				Vector3 wallPos = board.GetCoordinates(r, c);
				GameObject wall = Instantiate(
					prefabWall, wallPos, Quaternion.identity);
				board.Set(r, c, wall);
			}

			// available player classes
			string[] playerAvailableCharacters = reader.ReadLine().Split(
				default(char[]),
				StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < numPlayerC; i++) {
				GameObject prefab = null;

				switch (playerAvailableCharacters[i][0]) {
					case 'A':
                        prefab = prefabArcher;
						break;
					case 'T':
						break;
					case 'W':
						prefab = prefabWarrior;
						break;
				}

				playerAvailableCharactersPrefabs.Add(prefab);
			}

			// spawn points
			for (int i = 0; i < numPlayerC; i++) {
				string[] spawnCoords = reader.ReadLine().Split(
					default(char[]),
					StringSplitOptions.RemoveEmptyEntries);
				int r = Convert.ToInt32(spawnCoords[0]);
				int c = Convert.ToInt32(spawnCoords[1]);
				Vector3 spawnPos = board.GetCoordinates(r, c);
				spawnPos += 0.1f * Vector3.up;
				GameObject objSP = Instantiate(
					prefabSpawnPoint,
					spawnPos,
					prefabSpawnPoint.transform.rotation);
				PlayerSpawnPoint spawn = objSP.GetComponent<PlayerSpawnPoint>();
				spawn.r = r;
				spawn.c = c;
				playerSpawnPoints.Add(spawn);
			}
		}

		// TODO parse enemies
		// TODO parse hazards

		levelToLoad = null;
		Init();
	}*/

	public void SceneLoaded(Scene scene, LoadSceneMode mode) {
		SceneManager.sceneLoaded -= SceneLoaded;
		board = GameObject.FindObjectOfType<Board>();
		Level level = Resources.Load("Levels/" + levelToLoad) as Level;
		levelName = level.title;
		timeLimit = level.timeLimit;
		playerSpawnPoints = new List<PlayerSpawnPoint>();
		foreach (Level.PlayerClass cls in level.classes) {
			GameObject prefab = null;
			switch (cls) {
				case Level.PlayerClass.ARCHER:
					prefab = prefabArcher;
					break;
				case Level.PlayerClass.THIEF:
					break;
				case Level.PlayerClass.WARRIOR:
					prefab = prefabWarrior;
					break;
			}
			playerAvailableCharactersPrefabs.Add(prefab);
		}
		foreach (Position pos in level.walls) {
			Vector3 wallPos = board.GetCoordinates(pos.row, pos.col);
			GameObject wall = Instantiate(
				prefabWall, wallPos, Quaternion.identity);
			board.Set(pos.row, pos.col, wall);
		}
		foreach (Position pos in level.spawnPoints) {
			Vector3 spawnPos = board.GetCoordinates(pos.row, pos.col);
			spawnPos += 0.1f * Vector3.up;
			GameObject objSP = Instantiate(
				prefabSpawnPoint,
				spawnPos,
				prefabSpawnPoint.transform.rotation);
			PlayerSpawnPoint spawn = objSP.GetComponent<PlayerSpawnPoint>();
			spawn.r = pos.row;
			spawn.c = pos.col;
			playerSpawnPoints.Add(spawn);
		}
		levelToLoad = null;
		Init();
	}

	public void Load(string levelName) {
		levelToLoad = levelName;
		SceneManager.sceneLoaded += SceneLoaded;
		state = State.LOADING;
		SceneManager.LoadScene("BaseLevel", LoadSceneMode.Single);
	}

	public void Register(Actor.Action action) {
		if (selectedActor != null) {
			selectedActor.AddAction(action);
		}
	}

	public void StartLoop() {
		selectedActor = null;
		actors.ForEach(actor => actor.BeginPlan());
		state = State.RUNNING;
		StartCoroutine(StepLoop());
	}

	IEnumerator StepLoop() {
		// TODO this is definitely not a while true
		while (actors.Any(actor => !actor.done)) {
			if (actors.All(actor => actor.ready || actor.done)) {
				// actors who have more stuff to do
				actors
					.ForEach(actor => actor.NextAction());
				List<Actor> notDone = actors
					.Where(actor => actor.ready && !actor.done)
					.ToList<Actor>();
				List<Actor> needUpdate;
				List<Actor> neededUpdateLastIter = notDone;
				bool done;
				// iterate through all actors until those who could eventually
				// perform their actions are able to do so
				do {
					// filter out those who managed to perform their action
					needUpdate = neededUpdateLastIter
						.Where(actor => !actor.PerformAction())
						.ToList<Actor>();
					// check if anything changed this iteration
					done = needUpdate.Count == neededUpdateLastIter.Count;
					neededUpdateLastIter = needUpdate;
				} while (!done);
				needUpdate
					.ForEach(actor => actor.LookAtTargetPos());
			}
			yield return new WaitForSeconds(stepLoopDelay);
		}
	}

	public void ResetRoom() {
		board.ClearActors();
		actors.ForEach(
			act => {
				act.Restart();
				act.ClearActions();
				// TODO rotate actors back to original orientation
			});
		state = State.PLANNING;
	}
}
