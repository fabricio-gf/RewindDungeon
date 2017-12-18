using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

	public enum Action {
		MOVE_U,
		MOVE_D,
		MOVE_L,
		MOVE_R,
		ATTACK
	}

	public float movementTime = 0.5f;
	public float rotationTime = 0.2f;


	public int initR;
	public int initC;

	public int r;
	public int c;

	public bool ready;
	public bool done;

	public List<Action> plan;

	public IEnumerator<Action> actions;

	private Board board;

    public ActorInfo info;

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
		if (plan.Count > 0) {
			actions = plan.GetEnumerator();
			ready = true;
			done = false;
		} else {
			actions = null;
			ready = false;
			done = true;
		}
	}

	public void Restart() {
		this.r = this.initR;
		this.c = this.initC;
		board.Set(r, c, gameObject);
		transform.position = board.GetCoordinates(r, c);
		this.SetReady();
	}

	public void AddAction(Action a) {
		actions = null;
		plan.Add(a);
	}

	public void ClearActions() {
		plan.Clear();
	}

	public bool NextAction() {
		if (!ready) {
			print("NOT READY");
			return false;
		}
		done = !actions.MoveNext();
		return !done;
	}

	public bool PerformAction() {
		int nr, nc;
		switch (actions.Current) {
			case Action.MOVE_U:
			case Action.MOVE_D:
			case Action.MOVE_L:
			case Action.MOVE_R:
				NextPos(out nr, out nc);
				return TryMoveTo(nr, nc);
			case Action.ATTACK:
				// TODO attack
				break;
		}
		return false;
	}

	private bool TryMoveTo(int nr, int nc) {
		if (board.Move(r, c, nr, nc)) {
			r = nr;
			c = nc;
			AnimateMovement();
			return true;
		}
		return false;
	}

	private bool NextPos(out int nr, out int nc) {
		nr = r;
		nc = c;
		switch (actions.Current) {
			case Action.MOVE_U:
				nr = r-1;
				return true;
			case Action.MOVE_D:
				nr = r+1;
				return true;
			case Action.MOVE_L:
				nc = nc-1;
				return true;
			case Action.MOVE_R:
				nc = nc+1;
				return true;
		}
		return false;
	}

	public void LookAtTargetPos() {
		int nr, nc;
		if (NextPos(out nr, out nc)) {
			ready = false;
			Vector3 pos = board.GetCoordinates(nr, nc);
			iTween.LookTo(
				gameObject,
				iTween.Hash(
					"looktarget", pos,
					"axis", "y",
					"delay", 0,
					"time", rotationTime,
					"oncomplete", "SetReady"));
		}
	}

	private void AnimateMovement() {
		ready = false;
		Vector3 pos = board.GetCoordinates(r, c);
		iTween.LookTo(
			gameObject,
			iTween.Hash(
				"looktarget", pos,
				"axis", "y",
				"delay", 0,
				"time", rotationTime,
				"oncomplete", "EndTurning"));
	}

	void EndTurning() {
		Vector3 pos = board.GetCoordinates(r, c);
		iTween.MoveTo(
			gameObject,
			iTween.Hash(
				"x", pos.x,
				"z", pos.z,
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
