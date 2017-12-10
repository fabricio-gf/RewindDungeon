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

}
