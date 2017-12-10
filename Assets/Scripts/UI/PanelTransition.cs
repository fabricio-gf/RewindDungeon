using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTransition : MonoBehaviour {

	public Vector2 InitialPos;
	public Vector2 FinalPos;
	public float EntryTime;
	public float ExitTime;

	void Awake() {
		InitialPos = transform.TransformPoint(new Vector3(InitialPos.x, InitialPos.y, 0));
		FinalPos = transform.TransformPoint(new Vector3(FinalPos.x, FinalPos.y, 0));
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
