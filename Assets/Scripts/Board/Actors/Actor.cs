using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {
	public virtual void Test() {}

	public enum Action {
		MOVE_U,
		MOVE_D,
		MOVE_L,
		MOVE_R,
		ATTACK
	}

	public float movementTime = 0.5f;


	public int initR;
	public int initC;

	public int r;
	public int c;

	public bool ready;

	public List<Action> plan;

	private IEnumerator<Action> actions;

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
	}

	public void BeginPlan() {
		actions = plan.GetEnumerator();
		ready = true;
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
		if (!ready) {
			return false;
		}
		return actions.MoveNext();
	}

	public bool PerformAction() {
		switch (actions.Current) {
			case Action.MOVE_U:
				if (board.Move(r, c, r-1, c)) {
					r--;
					this.AnimateMovement();
					return true;
				}
				break;
			case Action.MOVE_D:
				if (board.Move(r, c, r+1, c)) {
					r++;
					this.AnimateMovement();
					return true;
				}
				break;
			case Action.MOVE_L:
				if (board.Move(r, c, r, c-1)) {
					c--;
					this.AnimateMovement();
					return true;
				}
				break;
			case Action.MOVE_R:
				if (board.Move(r, c, r, c+1)) {
					c++;
					this.AnimateMovement();
					return true;
				}
				break;
			case Action.ATTACK:
				// TODO attack
				break;
		}
		return false;
	}

	private void AnimateMovement() {
		ready = false;
		Vector3 realPos = board.GetCoordinates(r, c);
		iTween.MoveTo(
			gameObject,
			iTween.Hash(
				"x", realPos.x,
				"z", realPos.z,
				"easetype", "easeOutQuad",
				"orienttopath", true,
				"delay", 0,
				"time", movementTime,
				"oncomplete", "SetReady"));
	}

	private void SetReady() {
		ready = true;
	}

}
