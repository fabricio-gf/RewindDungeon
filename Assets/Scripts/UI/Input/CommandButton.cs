using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButton : MonoBehaviour {

	public Actor.Action action;

	public void OnClick() {
		GameManager.GM.Register(action);
	}

}
