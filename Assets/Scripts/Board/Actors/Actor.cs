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
	public float rotationTime = 0.2f;

	public int initR;
	public int initC;

	public int r;
	public int c;

	public bool ready;
	public bool done;
    public bool hasTakenDamage;

    public bool isArcher;
    public bool isWarrior;
    public GameObject arrow;

	public List<Action> plan;

    public int maxActions;
    //public IEnumerator<Action> actions;
    public int actionIndex;
    Action lastAction;

	private Board board;

    public ActorInfo info;

    public GameObject preview;

	public void Spawn(Board board, int initR, int initC) {
		plan = new List<Action>();

		this.board = board;
		this.r = initR;
		this.c = initC;
		this.initR = initR;
		this.initC = initC;
		board.Set(initR, initC, gameObject);

		transform.position = board.GetCoordinates(initR, initC);

        if(GetComponent<PlayerCharacter>() != null && preview == null)
        {
            preview = Instantiate(GameManager.GM.PreviewToSpawn, this.transform);
            ShowPreview();
        }
	}

	public void BeginPlan() {
        if (isArcher)
        {
            lastAction = Action.MOVE_U;
            AddAction(Action.SHOOT);
        }
        if (plan.Count > 0) {
            //actions = plan.GetEnumerator();
            actionIndex = -1;
			ready = true;
			done = false;
		} else {
            //actions = null;
            actionIndex = -1;
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
            //actions = null;
            actionIndex = -1;
            plan.Add(a);

            if(a != Action.SHOOT)
            {
                UpdatePreview();
            }
        }
        else
        {
            //cannot add any more actions
        }
	}

	public void ClearActions() {
		plan.Clear();
	}

    void UpdatePreview()
    {
        int nr, nc;
        nr = r;
        nc = c;
        for(int i = 0; i < plan.Count; i++)
        {
            NextPos(nr, nc, plan[i], out nr, out nc);
        }

        preview.transform.position = board.GetCoordinates(nr, nc);
    }

    public void ShowPreview() {

        if (preview != null)
            preview.SetActive(true);
        else
            print("deu merda");
    }

    public void HidePreview()
    {
        if (preview != null)
            preview.SetActive(false);
        else
            print("deu merda");
    }

    public bool NextAction(bool force=false) {
		if (done || !force && !ready) {
			print("NOT READY");
			return false;
		}
        //lastAction = actions.Current;
        actionIndex++;
        if (actionIndex >= plan.Count)
        {
            done = true;
            return false;
        }
        if (actionIndex > 0)
        {
            lastAction = plan[actionIndex-1];
        }
		return true;
	}

	public bool PerformAction() {
		int nr, nc;
		//switch (actions.Current) {
        switch (plan[actionIndex]) {
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
                ready = false;
                NextPos(lastAction, out nr, out nc);
                if (board.WithinBounds(nr, nc)) {
                    GameObject obj = Instantiate(arrow, board.GetCoordinates(nr, nc), arrow.transform.rotation);
                    obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, transform.eulerAngles.y, obj.transform.eulerAngles.z);
                    Arrow arrowScript = obj.GetComponent<Arrow>();
                    arrowScript.archerParent = this;
                    obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * arrowScript.speed;
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
            } else if (this.CompareTag("Player") && obj.CompareTag("Enemy") && !this.GetComponent<Actor>().hasTakenDamage)
            {
                print("entrou aqui tmb");
                //TakeDamage();
            }
        }
        return false;
    }

    private bool NextPos(int r, int c, Action action, out int nr, out int nc)
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

	private bool NextPos(out int nr, out int nc) {
        return NextPos(plan[actionIndex], out nr, out nc);
	}

    private bool NextPos(Action action, out int nr, out int nc)
    {
        return NextPos(r, c, action, out nr, out nc);
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
                "name", "movement",
				"oncomplete", "EndMoving"));
	}

    void EndMoving()
    {
        print("end");
        if (isArcher && actionIndex < plan.Count-1 && plan[actionIndex+1] == Action.SHOOT)
        {
            NextAction(true);
            PerformAction();
        } else {
            SetReady();
        }
    }

	public void SetReady() {
        hasTakenDamage = false;
		ready = true;
	}

    public void TakeDamage()
    {
        if (isWarrior)
        {
            
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        //ativar animação de morte
        done = true;
        board.Set(r, c, null);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Button")) {
            col.gameObject.GetComponent<ButtonBehaviour>().Trigger();
        }
    }


}
