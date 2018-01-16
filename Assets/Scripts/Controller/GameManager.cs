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

	public GameObject prefabSpawnPoint;
	public GameObject prefabTestEnemy;
    public GameObject prefabCoin;
    public GameObject prefabExit;

    public GameObject CharacterToSpawn;

	public List<Actor> actors;
	public List<GameObject> playerAvailableCharactersPrefabs;
	public List<PlayerSpawnPoint> playerSpawnPoints;

	public string levelToLoad;
	public string levelName;
	public int timeLimit;

	public Board board;
	public State state;

	[SerializeField]
	private Actor _selectedActor;
	public Actor selectedActor {
		get {
			return _selectedActor;
		}
		set {
			if (_selectedActor != null) {
				_selectedActor.transform.Find("Body")
					.GetComponent<Renderer>().material
					.SetColor("_OutlineColor", Color.clear);
			}
			_selectedActor = value;
			if (_selectedActor != null) {
				_selectedActor.transform.Find("Body")
					.GetComponent<Renderer>().material
					.SetColor("_OutlineColor", Color.red);
			}
		}
	}
    public CharacterButton selectedButton;

    public bool hasReachedExit;

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
        Deselect();
	}

	public void Init() {
		state = State.PLANNING;
	}

	public void ExitLevel() {
		state = State.OUT_OF_LEVEL;
		// TODO return to level select
	}

	public void SceneLoaded(Scene scene, LoadSceneMode mode) {
		SceneManager.sceneLoaded -= SceneLoaded;
		StartCoroutine(InitLevel());
	}

	IEnumerator InitLevel() {
		yield return new WaitForEndOfFrame();
		board = GameObject.FindWithTag("Board").GetComponent<Board>();
		Level level = Resources.Load("Levels/" + levelToLoad) as Level;
		levelName = level.title;
		timeLimit = level.timeLimit;

		playerSpawnPoints = new List<PlayerSpawnPoint>();
		actors = new List<Actor>();

        playerAvailableCharactersPrefabs = new List<GameObject>();

        foreach (Level.PlayerClass cls in level.classes) {
			GameObject prefab = null;
			switch (cls) {
				case Level.PlayerClass.ARCHER:
					prefab = prefabArcher;
					break;
				case Level.PlayerClass.THIEF:
                    prefab = prefabArcher;
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
        foreach (Position pos in level.coins)
        {
            Vector3 coinPos = board.GetCoordinates(pos.row, pos.col);
            GameObject coin = Instantiate(
                prefabCoin, coinPos, Quaternion.identity);
            //board.Set(pos.row, pos.col, coin);
        }
        foreach (Level.EnemyInstance inst in level.enemies) {
			GameObject prefab = null;
			switch (inst.enemyType) {
				case Level.EnemyType.TEST_ENEMY:
					prefab = prefabTestEnemy;
					break;
			}
			Actor actor = Instantiate(prefab).GetComponent<Actor>();
			actor.Spawn(board, inst.position.row, inst.position.col);
			actor.transform.Translate(new Vector3(0, 0.5f, 0));
			inst.plan.ForEach(
				action => actor.AddAction(action));
			actors.Add(actor);
		}
        Vector3 exitPos = board.GetCoordinates(level.exit.row, level.exit.col);
        GameObject exit = Instantiate(
            prefabExit, exitPos, Quaternion.identity);
        //board.Set(level.exit.row, level.exit.col, exit);

        //levelToLoad = null;
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
        Deselect();
		actors.ForEach(actor => actor.BeginPlan());
		state = State.RUNNING;
		StartCoroutine(StepLoop());
	}

	void Update() {
		if (Input.GetKeyDown("a")) {
			actors.ForEach(actor => actor.BeginPlan());
		}
		if (Input.GetKeyDown("space")) {
			// yes this is a copy paste, shut up
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
		}
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
        if (hasReachedExit)
        {
            print("victory");
            Victory();
        }
	}

    public void Deselect()
    {
        selectedActor = null;
        selectedButton = null;
    }

	public void ResetRoom() {
        /*board.ClearActors();
		actors.ForEach(
			act => {
				act.Restart();
				act.ClearActions();
				// TODO rotate actors back to original orientation
			});
		state = State.PLANNING;*/

        Load(levelToLoad);
        //StartCoroutine(InitLevel());
	}

    public void Victory()
    {
        //parar execução e não receber mais inputs
        //mostrar ui de vitoria
    }
}
