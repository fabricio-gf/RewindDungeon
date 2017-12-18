using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	public GameObject panel;

	GameObject startObj;

	void Update() {
		if (!panel.activeInHierarchy && Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				startObj = Raycast(touch);
			}
			if (touch.phase == TouchPhase.Ended) {
				GameObject endObj = Raycast(touch);
				if (endObj == startObj && endObj != null) {
					RaycastTarget target = endObj.GetComponent<RaycastTarget>();
					if (target != null) {
						target.Click();
					}
				}
			}
		}
	}

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
}
