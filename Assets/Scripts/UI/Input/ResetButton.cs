using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {

    public InputManager inputManager;

	public void ResetAll() {
		GameManager.GM.ResetRoom();
	}

    public void ResetActor()
    {
        GameManager.GM.selectedActor.GetComponent<PlayerCharacter>().Remove();
        GameManager.GM.selectedButton.Available = true;
        GameManager.GM.selectedButton.UpdateUI();
        GameManager.GM.Deselect();
        inputManager.OpenPanel("CharacterSelection");
    }

}
