using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class PlayerCharacter : RaycastTarget {

	Actor actor;
    public PlayerSpawnPoint SpawnPoint;

	// Use this for initialization
	void Awake() {
		actor = GetComponent<Actor>();
	}

	public override void Click() {
		// TODO add way to deselect actor
		if (GameManager.GM.state == GameManager.State.PLANNING) {
			GameManager.GM.selectedActor = actor;
		}
	}

    public void Remove()
    {
        SpawnPoint.gameObject.SetActive(true);
        Destroy(gameObject);
    }


}
