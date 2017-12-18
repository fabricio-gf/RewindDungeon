using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlanButton : MonoBehaviour {
	
	public void Click() {
		if (GameManager.GM.state == GameManager.State.PLANNING) {
			GameManager.GM.StartLoop();
		}
	}
}
