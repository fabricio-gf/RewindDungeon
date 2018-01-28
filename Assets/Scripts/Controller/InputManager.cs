using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    public GameObject CharacterSelection;

    public GameObject RestartButton;

	GameObject startObj;

    public GameObject[] Buttons;

	GameObject Raycast(Touch touch) {
		Ray ray = Camera.main.ScreenPointToRay(touch.position);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			return hit.transform.gameObject;
		}
		return null;
	}

    public void OpenPanel(string panelName)
    {
        transform.parent.Find(panelName).gameObject.SetActive(true);
    }

    public void OpenCharacterSelection(string panelName)
    {
        transform.parent.Find(panelName).GetChild(1).GetComponent<PanelTransition>().Enable();
        transform.parent.Find(panelName).GetChild(1).GetChild(0).gameObject.SetActive(true);
        transform.parent.Find(panelName).GetChild(1).GetChild(1).gameObject.SetActive(false);

    }

    public void ActivateButtons()
    {
        for(int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].SetActive(true);
        }
    }

    public void DeactivateButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].SetActive(false);
        }
    }

    public void PlayButtonSound()
    {
        SoundManager.SM.ButtonSound();
    }

    public void PlayPanelSound()
    {
        SoundManager.SM.PanelSound();
    }
}
