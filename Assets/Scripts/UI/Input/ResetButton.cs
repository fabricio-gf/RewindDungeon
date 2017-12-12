using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {

	public void Click() {
		GameManager.LM.ResetRoom();
	}

}
