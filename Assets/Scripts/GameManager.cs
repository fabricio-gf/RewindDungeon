using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public List<Actor> actors;
	public GameObject prefabActor;

	public string levelName;

	Board board;
	bool running;
	int selectedActor = 0;

	void Awake() {
		board = GameObject.FindObjectOfType<Board>();

		board.Init();

		running = false;
		actors = new List<Actor>();

		for (int i = 0; i < 2; i++) {
			GameObject obj = Instantiate(
				prefabActor, Vector3.zero, Quaternion.identity);
			Actor act = obj.GetComponent<Actor>();
			actors.Add(act);
			act.Spawn(board, 0, i);
		}
	}

	void Update() {
		if (Input.GetKeyDown("left")) {
			actors[selectedActor].AddAction(Actor.Action.MOVE_L);
		} else if (Input.GetKeyDown("right")) {
			actors[selectedActor].AddAction(Actor.Action.MOVE_R);
		} else if (Input.GetKeyDown("up")) {
			actors[selectedActor].AddAction(Actor.Action.MOVE_U);
		} else if (Input.GetKeyDown("down")) {
			actors[selectedActor].AddAction(Actor.Action.MOVE_D);
		}
		if (Input.GetKeyDown("tab")) {
			selectedActor = (selectedActor + 1) % actors.Count;
		}

		if (Input.GetKey("space")) {
			// check if every actor can perform the next move
			if (actors.All(act => act.ready)) {
				if (!running) {
					running = true;
					actors.ForEach(act => act.BeginPlan());
				}
				actors.ForEach(act => act.NextAction());
			}
		}
		if (Input.GetKeyDown("r")) {
			if (actors.All(act => act.ready)) {
				ResetRoom();
			}
		}
		if (Input.GetKeyDown("c")) {
			SaveLevel("0");
		}
		if (Input.GetKeyDown("l")) {
			LoadLevel("0");
		}
	}

	void ResetRoom() {
		board.ClearActors();
		actors.ForEach(
			act => {
				act.Restart();
				act.ClearActions();
			});
		running = false;
	}

	public void SaveLevel(string levelName) {
		Level l = new Level();
		l.timeLimit = 10;
		l.playerCharacters.Add(actors[0]);
		print(JsonUtility.ToJson(l.playerCharacterList.actors[0], true));
		print(JsonUtility.ToJson(l, true));
	}

	public void LoadLevel(string levelName) {
		
	}

}
