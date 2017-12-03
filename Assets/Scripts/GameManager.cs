using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public List<Actor> actors;
	public GameObject prefabActor;

	Board board;
	bool running;

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
			actors[0].AddAction(Actor.Action.MOVE_L);
		} else if (Input.GetKeyDown("right")) {
			actors[0].AddAction(Actor.Action.MOVE_R);
		} else if (Input.GetKeyDown("up")) {
			actors[0].AddAction(Actor.Action.MOVE_U);
		} else if (Input.GetKeyDown("down")) {
			actors[0].AddAction(Actor.Action.MOVE_D);
		}
		if (Input.GetKeyDown("a")) {
			actors[1].AddAction(Actor.Action.MOVE_L);
		} else if (Input.GetKeyDown("d")) {
			actors[1].AddAction(Actor.Action.MOVE_R);
		} else if (Input.GetKeyDown("w")) {
			actors[1].AddAction(Actor.Action.MOVE_U);
		} else if (Input.GetKeyDown("s")) {
			actors[1].AddAction(Actor.Action.MOVE_D);
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

}
