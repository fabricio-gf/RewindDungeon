using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameManager))]
public class LevelManager : MonoBehaviour {


	/*  LEVEL FILE FORMAT:
	 *  - All lower case identifiers indicate integers
	 *  - Single upper case letter identifiers indicate enumerations
	 *  	A \in {
	 *  		'U', for MOVE_U
	 *  		'D', for MOVE_D
	 *  		'L', for MOVE_L
	 *  		'R', for MOVE_R
	 *  		'S', for STOP (TODO)
	 *  		'A', for ATTACK
	 *  	}
	 *  	P \in {
	 *  		'A', for archer
	 *  		'T', for thief
	 *  		'W', for warrior
	 *   	}
	 *   	E \in {
	 *   		LIST OF ENEMY TYPES
	 *   	}
	 *   	H \in {
	 *   		LIST OF HAZARD TYPES
	 *   	}
	 * 
	 * LEVEL_NAME
	 * t
	 * num_w
	 * num_p
	 * num_e
	 * num_h
	 * w0r w0c
	 * w1r w1c
	 * ...
	 * P P P ...
	 * p0r p0c
	 * p1r p1c
	 * ...
	 * E e0r e0c A A ...
	 * E e1r e1c A A ...
	 * ...
	 * H h0r h0c
	 * H h1r h1c
	 * ...
	 */
	
	public enum State {
		OUT_OF_LEVEL,
		LOADING,
		PLANNING,
		RUNNING
	}

	public static LevelManager LM {
		get; private set;
	}
	
	public float stepLoopDelay = 0.1f;

	public GameObject prefabWall;
	public GameObject prefabWarrior;

	public GameObject prefabSpawnPoint;

	private GameManager gm;

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
		DontDestroyOnLoad(gameObject);
		gm = GetComponent<GameManager>();
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

	void Update() {
		if (state == State.PLANNING) {
			// TODO
			// if (Input.GetKeyDown("left")) {
			// 	selectedActor.AddAction(Actor.Action.MOVE_L);
			// } else if (Input.GetKeyDown("right")) {
			// 	selectedActor.AddAction(Actor.Action.MOVE_R);
			// } else if (Input.GetKeyDown("up")) {
			// 	selectedActor.AddAction(Actor.Action.MOVE_U);
			// } else if (Input.GetKeyDown("down")) {
			// 	selectedActor.AddAction(Actor.Action.MOVE_D);
			// }

		} else if (state == State.RUNNING) {
			if (Input.GetKeyDown("r")) {
				// TODO clear plan for player characters only
				// if (actors.All(act => act.ready)) {
					// ResetRoom();
				// }
			}
		}
	}

	public void StartLoop() {
		actors.ForEach(actor => actor.BeginPlan());
		state = State.RUNNING;
		StartCoroutine(StepLoop());
	}

	IEnumerator StepLoop() {
		// TODO this is definitely not a while true
		while (true) {
			if (actors.All(actor => actor.ready)) {
				// actors who have more stuff to do
				List<Actor> notDone = actors.Where(
					actor => actor.NextAction())
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
			}
			yield return new WaitForSeconds(stepLoopDelay);
		}
	}

	void ResetRoom() {
		board.ClearActors();
		actors.ForEach(
			act => {
				act.Restart();
				act.ClearActions();
			});
		state = State.PLANNING;
	}
}
