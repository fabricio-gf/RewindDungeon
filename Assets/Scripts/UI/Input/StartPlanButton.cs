using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlanButton : MonoBehaviour {
	
	public void Click() {
		if (GameManager.LM.state == LevelManager.State.PLANNING) {
			GameManager.LM.StartLoop();
		}
	}
}
