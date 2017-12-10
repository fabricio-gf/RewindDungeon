using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButton : MonoBehaviour {

	public Actor.Action action;

	LevelManager lm;

	void Start() {
		lm = GameObject.FindObjectOfType<LevelManager>();
	}

	public void OnClick() {
		lm.Register(action);
	}

}
