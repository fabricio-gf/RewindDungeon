using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : RaycastTarget {

	public int r;
	public int c;

	LevelManager lm;

	void Start() {
		lm = GameObject.FindObjectOfType<LevelManager>();
	}

	override public void Click() {
		// TODO abrir menu de seleção de personagens

		print("(" + r + ", " + c + ")");
	}

	public GameObject Spawn(GameObject characterPrefab) {
		GameObject obj = Instantiate(
			characterPrefab,
			lm.board.GetCoordinates(r, c),
			characterPrefab.transform.rotation);
		lm.board.Set(r, c, obj);
		return obj;
	}

}
