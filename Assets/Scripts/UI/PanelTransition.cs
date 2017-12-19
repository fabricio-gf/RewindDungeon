using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTransition : MonoBehaviour {

    [Header ("TRANSITION VARIABLES")]
	public Vector3 InitialPos;
	public Vector3 FinalPos;
	public float EntryTime;
	public float ExitTime;

    /// <summary>
    /// Converts initial and final positions to global space from local space
    /// </summary>
	void Awake() {
		InitialPos = transform.TransformPoint(InitialPos);
        FinalPos = transform.TransformPoint(FinalPos);
	}
	
    /// <summary>
    /// When the object is set to active, moves from initial position to final position, easing at the start and end with iTween
    /// </summary>
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
	
    /// <summary>
    /// Moves out of screen back to initial position and calls Closed method
    /// </summary>
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
	
    /// <summary>
    /// Deactivates the panel
    /// </summary>
	void Closed(){
		transform.position = InitialPos;
		transform.parent.gameObject.SetActive(false);
	}
}
