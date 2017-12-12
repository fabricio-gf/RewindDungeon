using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlanButton : MonoBehaviour {
	
	public void Click() {
		GameManager.LM.StartLoop();
	}
}
