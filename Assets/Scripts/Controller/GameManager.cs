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

    [Header("PREFABS")]
	//public GameObject prefabWall;
    [Space(20)]
	public GameObject prefabWarrior;
    public GameObject prefabArcher;
    public GameObject prefabThief;

    public GameObject previewWarrior;
    public GameObject previewArcher;
    public GameObject previewThief;
    [Space(20)]
	public GameObject prefabSpawnPoint;
	public GameObject prefabTestEnemy;
    public GameObject prefabCoin;
    public GameObject prefabExit;
    public GameObject prefabButton;
    public GameObject prefabDoor;

    [Space(20)]
    public GameObject[] prefabTile;
    public GameObject[] prefabWall;
    public float wallMaxRotation = 45.0f;
    public float wallHeightAdjustBase;
    public float wallHeightAdjustWindow;

    [Header("OTHER ATTRIBUTES")]
    public GameObject CharacterToSpawn;
    public GameObject PreviewToSpawn;

	public List<Actor> actors;
	public List<GameObject> playerAvailableCharactersPrefabs;
    public CharacterSelect characterSelectPanel;
	public List<PlayerSpawnPoint> playerSpawnPoints;

	public string levelToLoad;
	public string levelName;
	public int timeLimit;

	public Board board;
	public State state;

    public GameObject victoryPanel;

	[SerializeField]
	private Actor _selectedActor;
	public Actor selectedActor {
		get {
			return _selectedActor;
		}
		set {
			if (_selectedActor != null) {
				_selectedActor.selectionArrow.SetActive(false);
				//_selectedActor.transform.Find("Body")
				//	.GetComponent<Renderer>().material
				//	.SetColor("_OutlineColor", Color.clear);
                _selectedActor.HidePreview();
			}
			_selectedActor = value;
			if (_selectedActor != null) {
				_selectedActor.selectionArrow.SetActive(true);
				//_selectedActor.transform.Find("Body")
				//	.GetComponent<Renderer>().material
				//	.SetColor("_OutlineColor", Color.red);
                _selectedActor.ShowPreview();
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
		hasReachedExit = false;
		board = GameObject.FindWithTag("Board").GetComponent<Board>();
        characterSelectPanel = GameObject.FindWithTag("CharacterSelect").GetComponent<CharacterSelect>();
        Level level = Resources.Load("Levels/" + levelToLoad) as Level;
		levelName = level.title;
		timeLimit = level.timeLimit;

        victoryPanel = GameObject.FindGameObjectWithTag("VictoryPanel").transform.GetChild(0).gameObject;

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

        System.Random rng = new System.Random();
        characterSelectPanel.Init();
        foreach (Position pos in level.walls) {
			Vector3 wallPos = board.GetCoordinates(pos.row, pos.col);
			CreateWall(rng, pos.row, pos.col);
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
            coinPos += 0.5f * Vector3.up;
            GameObject coin = Instantiate(
                prefabCoin, coinPos, prefabCoin.transform.rotation);
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
        if (level.button.row != -1 && level.button.col != -1 && level.door.row != -1 && level.door.col != -1)
        {
            Vector3 doorPos = board.GetCoordinates(level.door.row, level.door.col);
            GameObject door = Instantiate(
                prefabDoor, doorPos, Quaternion.identity);
            board.Set(level.door.row, level.door.col, door);

            Vector3 buttonPos = board.GetCoordinates(level.button.row, level.button.col);
            GameObject button = Instantiate(
                prefabButton, buttonPos, Quaternion.identity);
            button.GetComponent<ButtonBehaviour>().door = door;
            button.GetComponent<ButtonBehaviour>().targetCol = level.door.col;
            button.GetComponent<ButtonBehaviour>().targetRow = level.door.row;

        }

        for (int i = 0; i < Board.GRID_ROWS; i++) {
        	for (int j = 0; j < Board.GRID_COLS; j++) {
        		if (i != level.exit.row || j != level.exit.col) {
        			CreateFloorTile(rng, i, j);
        		}
        	}
        }
        Vector3 exitPos = board.GetCoordinates(level.exit.row, level.exit.col);
        GameObject exit = Instantiate(
            prefabExit, exitPos, Quaternion.identity);
        //board.Set(level.exit.row, level.exit.col, exit);

        //levelToLoad = null;
		Init();
	}

	void CreateFloorTile(System.Random rng, int row, int col) {
		int tileType = rng.Next(0, prefabTile.Length);
		int tileRot = rng.Next(0, 4);

		GameObject floorTile = Instantiate(
			prefabTile[tileType],
			board.GetCoordinates(row, col),
			Quaternion.identity);
		floorTile.transform.eulerAngles =
			new Vector3(90.0f, 0, tileRot * 90.0f);
	}

	void CreateWall(System.Random rng, int row, int col) {
		int wallType = rng.Next(0, prefabWall.Length);
		GameObject prefab = prefabWall[wallType];
		GameObject wall = Instantiate(
			prefab,
			board.GetCoordinates(row, col),
			prefab.transform.rotation);
		board.Set(row, col, wall);
		Vector3 newRotation = wall.transform.eulerAngles;
		float rotationDelta = (float) (2*rng.NextDouble() - 1) * wallMaxRotation;
		newRotation.y += rotationDelta;
		float heightDelta = (float) (2*rng.NextDouble() - 1) * wallHeightAdjustWindow;
		heightDelta += wallHeightAdjustBase;
		wall.transform.Translate(0, 0, heightDelta);
		wall.transform.eulerAngles = newRotation;
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            victoryPanel.SetActive(!victoryPanel.activeSelf);
        }
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
        else
        {
            GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>().RestartButton.SetActive(true);
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
        //desativar as outras UIs
        SoundManager.SM.FanfareSound();
        SoundManager.SM.PanelSound();
        victoryPanel.SetActive(true);
    }

}
