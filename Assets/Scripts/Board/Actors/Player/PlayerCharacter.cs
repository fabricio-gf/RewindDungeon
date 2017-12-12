using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class PlayerCharacter : RaycastTarget {

	Actor actor;

	// Use this for initialization
	void Awake() {
		actor = GetComponent<Actor>();
	}

	public override void Click() {
		// TODO add way to deselect actor
		GameManager.LM.selectedActor = actor;
	}
}
