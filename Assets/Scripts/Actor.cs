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

	public int initR, initC;
	public int r { get; private set; }
	public int c { get; private set; }

    private List<Action> plan;
	private IEnumerator<Action> actions;
	private Board board;

	public void Spawn(int initR, int initC) {
		plan = new List<Action>();
		board = GameObject.FindObjectOfType<Board>();

		this.initR = initR;
		this.initC = initC;
	}

	public void Restart() {
		r = initR;
		c = initC;
		actions = plan.GetEnumerator();
	}

    public void AddAction(Action a) {
		plan.Add(a);
	}

	public void ClearActions() {
		plan.Clear();
	}
	
    public bool NextAction() {
		if (!actions.MoveNext()) {
			return false;
		}
		// TODO iTween
		switch (actions.Current) {
			case Action.MOVE_U:
				if (board.Move(r, c, r-1, c)) {
					r--;
				}
				break;
			case Action.MOVE_D:
				if (board.Move(r, c, r+1, c)) {
					r++;
				}
				break;
			case Action.MOVE_L:
				if (board.Move(r, c, r, c-1)) {
					c--;
				}
				break;
			case Action.MOVE_R:
				if (board.Move(r, c, r, c+1)) {
					c++;
				}
				break;
			case Action.ATTACK:
				// TODO attack
				break;
		}
		return true;
    }

}
