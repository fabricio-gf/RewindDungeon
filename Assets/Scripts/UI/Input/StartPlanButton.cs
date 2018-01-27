using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlanButton : MonoBehaviour {
	
	public void Click() {
		if (GameManager.GM.state == GameManager.State.PLANNING) {
            GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>().DeactivateButtons();
            GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>().CharacterSelection.SetActive(false);
            GameManager.GM.StartLoop();
		}
	}
}
