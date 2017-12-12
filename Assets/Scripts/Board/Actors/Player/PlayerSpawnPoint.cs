using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : RaycastTarget {

	public int r;
	public int c;

	override public void Click() {
		// TODO abrir menu de seleção de personagens

		print("(" + r + ", " + c + ")");
	}

	public GameObject Spawn(GameObject characterPrefab) {
		GameObject obj = Instantiate(
			characterPrefab,
			GameManager.LM.board.GetCoordinates(r, c),
			characterPrefab.transform.rotation);
		Actor actor = obj.GetComponent<Actor>();
		actor.Spawn(GameManager.LM.board, r, c);
		GameManager.LM.actors.Add(actor);
		gameObject.SetActive(false);
		return obj;
	}

}
