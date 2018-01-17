using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetActor : MonoBehaviour {

    public InputManager inputManager;

    public void Reset()
    {
        GameManager.GM.selectedActor.GetComponent<PlayerCharacter>().Remove();
        GameManager.GM.selectedButton.Available = true;
        GameManager.GM.selectedButton.UpdateUI();
        GameManager.GM.Deselect();
        inputManager.OpenPanel("CharacterSelection");
    }
}
