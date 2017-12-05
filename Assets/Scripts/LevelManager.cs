using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public bool isInLevel;

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
	 * P p0r p0c
	 * P p1r p1c
	 * ...
	 * E e0r e0c A A ...
	 * E e1r e1c A A ...
	 * ...
	 * H h0r h0c
	 * H h1r h1c
	 * ...
	 */
	
	public GameObject wallPrefab;
	public GameObject prefabWarrior;

	private GameManager gm;

	public List<Actor> playerCharacters;

	public string levelToLoad;
	public string levelName;
	public int timeLimit;

	public Board board;

	bool running;
	int selectedActor = 0;

	void Awake() {
		DontDestroyOnLoad(gameObject);
		gm = GetComponent<GameManager>();
	}

	public void Init() {
		board.Init();
		running = false;
		playerCharacters = new List<Actor>();
	}

	public void ExitLevel() {
		isInLevel = false;
		// TODO return to level select
	}

	void SceneLoaded(Scene scene, LoadSceneMode mode) {
		SceneManager.sceneLoaded -= SceneLoaded;

		board = GameObject.FindObjectOfType<Board>();
		TextAsset ta = Resources.Load(
			"Levels/" + levelToLoad,
			typeof(TextAsset)) as TextAsset;

		using (StringReader reader = new StringReader(ta.text)) {
			levelName = reader.ReadLine();
			timeLimit = Convert.ToInt32(reader.ReadLine());
			int numWalls = Convert.ToInt32(reader.ReadLine());
			int numPlayerC = Convert.ToInt32(reader.ReadLine());
			int numEnemy = Convert.ToInt32(reader.ReadLine());
			int numHazard = Convert.ToInt32(reader.ReadLine());

			for (int i = 0; i < numWalls; i++) {
				string[] wallPosStr = reader.ReadLine().Split(
						default(char[]),
						StringSplitOptions.RemoveEmptyEntries);
				int r = Convert.ToInt32(wallPosStr[0]);
				int c = Convert.ToInt32(wallPosStr[1]);

				Vector3 wallPos = board.GetCoordinates(r, c);
				GameObject wall = Instantiate(
					wallPrefab, wallPos, Quaternion.identity);
			}

			for (int i = 0; i < numPlayerC; i++) {
				string[] playerArgs = reader.ReadLine().Split(
					default(char[]),
					StringSplitOptions.RemoveEmptyEntries);
				GameObject prefab = null;

				switch (playerArgs[0][0]) {
					case 'A':
						break;
					case 'T':
						break;
					case 'W':
						prefab = prefabWarrior;
						break;
				}
				int r = Convert.ToInt32(playerArgs[1]);
				int c = Convert.ToInt32(playerArgs[2]);

				Vector3 playerPos = board.GetCoordinates(r, c);
				Instantiate(prefab, playerPos, Quaternion.identity);
			}
		}

		levelToLoad = null;
		isInLevel = true;
		Init();
	}

	public void Load(string levelName) {
		levelToLoad = levelName;
		SceneManager.sceneLoaded += SceneLoaded;
		SceneManager.LoadScene("BaseLevel", LoadSceneMode.Single);
	}

	void Update() {
		if (isInLevel) {
			if (Input.GetKeyDown("left")) {
				playerCharacters[selectedActor].AddAction(Actor.Action.MOVE_L);
			} else if (Input.GetKeyDown("right")) {
				playerCharacters[selectedActor].AddAction(Actor.Action.MOVE_R);
			} else if (Input.GetKeyDown("up")) {
				playerCharacters[selectedActor].AddAction(Actor.Action.MOVE_U);
			} else if (Input.GetKeyDown("down")) {
				playerCharacters[selectedActor].AddAction(Actor.Action.MOVE_D);
			}
			if (Input.GetKeyDown("tab")) {
				selectedActor = (selectedActor + 1) % playerCharacters.Count;
			}

			if (Input.GetKey("space")) {
				// check if every actor can perform the next move
				if (playerCharacters.All(act => act.ready)) {
					if (!running) {
						running = true;
						playerCharacters.ForEach(act => act.BeginPlan());
					}
					playerCharacters.ForEach(act => act.NextAction());
				}
			}
			if (Input.GetKeyDown("r")) {
				if (playerCharacters.All(act => act.ready)) {
					ResetRoom();
				}
			}
		}
	}

	void ResetRoom() {
		board.ClearActors();
		playerCharacters.ForEach(
			act => {
				act.Restart();
				act.ClearActions();
			});
		running = false;
	}
}
