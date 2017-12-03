using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public Actor test;

	Board board;

	void Awake() {
		board = GameObject.FindObjectOfType<Board>();

		board.Init();
		test.Spawn(board, 0, 0);
		test.AddAction(Actor.Action.MOVE_R);
		test.AddAction(Actor.Action.MOVE_L);
		test.AddAction(Actor.Action.MOVE_R);
		test.BeginPlan();
	}

	void Update() {
		if (Input.GetKeyDown("space")) {
			test.NextAction();
		}
		if (Input.GetKeyDown("r")) {
			if (test.ready) {
				board.ClearActors();
				test.Restart();
				test.BeginPlan();
			}
		}
	}

}
