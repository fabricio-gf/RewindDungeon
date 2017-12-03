using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Actor : MonoBehaviour {

	public enum Action {
		MOVE_U,
		MOVE_D,
		MOVE_L,
		MOVE_R,
		ATTACK
	}

	public int initR;
	public int initC;

	[System.NonSerialized]
	public int r;

	[System.NonSerialized]
	public int c;

	[System.NonSerialized]
	public bool ready;

	private List<Action> plan;

	[System.NonSerialized]
	private IEnumerator<Action> actions;

	[System.NonSerialized]
	private Board board;

	public void Spawn(Board board, int initR, int initC) {
		plan = new List<Action>();

		this.board = board;
		this.r = initR;
		this.c = initC;
		this.initR = initR;
		this.initC = initC;
		board.Set(initR, initC, gameObject);

		transform.position = board.GetCoordinates(initR, initC);
		this.SetReady();
	}

	public void BeginPlan() {
		actions = plan.GetEnumerator();
	}

	public void Restart() {
		this.r = this.initR;
		this.c = this.initC;
		board.Set(r, c, gameObject);
		transform.position = board.GetCoordinates(r, c);
		this.SetReady();
	}

	public void AddAction(Action a) {
		plan.Add(a);
	}

	public void ClearActions() {
		plan.Clear();
	}
	
	public bool NextAction() {
		if (!this.ready || !actions.MoveNext()) {
			return false;
		}
		switch (actions.Current) {
			case Action.MOVE_U:
				if (board.Move(r, c, r-1, c)) {
					r--;
					this.AnimateMovement();
				}
				break;
			case Action.MOVE_D:
				if (board.Move(r, c, r+1, c)) {
					r++;
					this.AnimateMovement();
				}
				break;
			case Action.MOVE_L:
				if (board.Move(r, c, r, c-1)) {
					c--;
					this.AnimateMovement();
				}
				break;
			case Action.MOVE_R:
				if (board.Move(r, c, r, c+1)) {
					c++;
					this.AnimateMovement();
				}
				break;
			case Action.ATTACK:
				// TODO attack
				break;
		}
		return true;
	}

	private void AnimateMovement() {
		this.ready = false;
		Vector3 realPos = board.GetCoordinates(this.r, this.c);
		iTween.MoveTo(
			gameObject,
			iTween.Hash(
				"x", realPos.x,
				"z", realPos.z,
				"easetype", "easeOutQuad",
				"orienttopath", true,
				"delay", 0,
				"time", 0.5,
				"oncomplete", "SetReady"));
	}

	private void SetReady() {
		this.ready = true;
	}

}
