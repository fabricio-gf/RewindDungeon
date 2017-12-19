using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTransition : MonoBehaviour {

	public Vector3 InitialPos;
	public Vector3 FinalPos;
	public float EntryTime;
	public float ExitTime;

	void Awake() {
		InitialPos = transform.TransformPoint(InitialPos);
        FinalPos = transform.TransformPoint(FinalPos);
	}
	
	// Use this for initialization
	void OnEnable(){
		transform.position = InitialPos;

        iTween.MoveTo(
            gameObject,
            iTween.Hash(
                "x", FinalPos.x,
                "y", FinalPos.y,
                "easetype", "easeInOutQuad",
                "time", EntryTime));		
	}
	
	public void ClosePanel(){
		transform.position = FinalPos;
		iTween.MoveTo(
			gameObject,
			iTween.Hash(
				"x", InitialPos.x,
				"y", InitialPos.y,
                "easetype", "easeInOutQuad",
				"time", ExitTime,
				"oncomplete", "Closed"));
	}
	
	void Closed(){
		transform.position = InitialPos;
		transform.parent.gameObject.SetActive(false);
	}
}
