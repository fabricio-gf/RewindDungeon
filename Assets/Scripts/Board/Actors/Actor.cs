using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

	public enum Action {
		MOVE_U,
		MOVE_D,
		MOVE_L,
		MOVE_R,
		ATTACK,
        SHOOT
	}

	public float movementTime = 0.5f;
    public float knockBackTime = 0.3f;
	public float rotationTime = 0.2f;

	public int initR;
	public int initC;

	public int r;
	public int c;

	public bool ready;
	public bool done;
    public bool hasTakenDamage;

    public bool isArcher;
    public GameObject arrow;

	public List<Action> plan;

    [Range(0,2)]
    public int hp;
    public int maxActions;
	public IEnumerator<Action> actions;
    Action lastAction;

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
        if (isArcher)
        {
            lastAction = Action.MOVE_U;
            AddAction(Action.SHOOT);
        }
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
        if (plan.Count < maxActions)
        {
            actions = null;
            plan.Add(a);
        }
        else
        {
            //cannot add any more actions
        }
	}

	public void ClearActions() {
		plan.Clear();
	}

	public bool NextAction() {
		if (!ready) {
			print("NOT READY");
			return false;
		}
        lastAction = actions.Current;
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
            case Action.SHOOT:

                NextPos(lastAction, out nr, out nc);
                if (board.WithinBounds(nr, nc)) {
                    GameObject obj = Instantiate(arrow, board.GetCoordinates(nr, nc), arrow.transform.rotation);
                    obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, transform.eulerAngles.y, obj.transform.eulerAngles.z);
                    Actor actor = obj.GetComponent<Actor>();
                    actor.Spawn(GameManager.GM.board, nr, nc);
                    GameManager.GM.actors.Add(actor);
                    for (int i = 0; i < 8; i++)
                    {
                        actor.AddAction(lastAction);
                    }
                    actor.BeginPlan();
                }
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
        else if (board.WithinBounds(nr, nc))
        {
            GameObject obj = GameManager.GM.board.Get(nr, nc);
            if(this.CompareTag("Enemy") && obj.CompareTag("Player") && !obj.GetComponent<Actor>().hasTakenDamage)
            {
                print("entrou");
                //animação de ataque
                obj.GetComponent<Actor>().TakeDamage();
                if (board.Move(r, c, nr, nc))
                {
                    r = nr;
                    c = nc;
                    AnimateMovement();
                    return true;
                }
            }

            if (this.CompareTag("Player") && obj.CompareTag("Enemy") && !this.GetComponent<Actor>().hasTakenDamage)
            {
                print("entrou aqui tmb");
                //TakeDamage();
            } 
        }
        return false;
    }

	private bool NextPos(out int nr, out int nc) {
        return NextPos(actions.Current, out nr, out nc);
	}

    private bool NextPos(Action action, out int nr, out int nc)
    {
        nr = r;
        nc = c;
        switch (action)
        {
            case Action.MOVE_U:
                nr = r - 1;
                return true;
            case Action.MOVE_D:
                nr = r + 1;
                return true;
            case Action.MOVE_L:
                nc = c - 1;
                return true;
            case Action.MOVE_R:
                nc = c + 1;
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

    private void KnockBackMovement(int nr, int nc)
    {
        Vector3 pos = board.GetCoordinates(nr, nc);
        r = nr;
        c = nc;
        print(pos);
        iTween.MoveTo(
            gameObject,
            iTween.Hash(
                "x", pos.x,
                "z", pos.z,
                "easetype", "easeOutCubic",
                "delay", 0,
                "time", knockBackTime,
                "oncomplete", "SetReady"));
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
                "name", "movement",
				"oncomplete", "SetReady"));
	}

	private void SetReady() {
        hasTakenDamage = false;
		ready = true;
	}

    public void TakeDamage()
    {
        hasTakenDamage = true;
        hp--;
        if (hp == 0)
        {
            Die();
        }
        else
        {
            //animação de tomar dano
            ready = false;
            //iTween.Stop(gameObject, "movement");
            int nr, nc;
            Action reverse;
            switch (actions.Current)
            {
                case Action.MOVE_U:
                    reverse = Action.MOVE_D;
                    break;
                case Action.MOVE_D:
                    reverse = Action.MOVE_U;
                    break;
                case Action.MOVE_L:
                    reverse = Action.MOVE_R;
                    break;
                case Action.MOVE_R:
                    reverse = Action.MOVE_L;
                    break;
                default:
                    reverse = Action.MOVE_U;
                    break;
            }
            NextPos(reverse, out nr, out nc);
            KnockBackMovement(nr, nc);
        }
    }

    private void Die()
    {
        //ativar animação de morte
        done = true;
        board.Set(r, c, null);
    }
}
