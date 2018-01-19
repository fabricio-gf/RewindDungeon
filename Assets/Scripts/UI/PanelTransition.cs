using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTransition : MonoBehaviour {

    [Header ("TRANSITION VARIABLES")]
	public Vector3 InitialPos;
	public Vector3 FinalPos;
	public float EntryTime;
	public float ExitTime;

    public bool startStill;
    bool dontDisable = false;

    /// <summary>
    /// Converts initial and final positions to global space from local space
    /// </summary>
    void Awake() {
		InitialPos = transform.TransformPoint(InitialPos);
        FinalPos = transform.TransformPoint(FinalPos);
	}

    public void Enable()
    {
        dontDisable = true;
        transform.parent.GetChild(0).gameObject.SetActive(true);
        OnEnable();
    }
	
    /// <summary>
    /// When the object is set to active, moves from initial position to final position, easing at the start and end with iTween
    /// </summary>
	void OnEnable(){
        if (!startStill)
        {
            transform.position = InitialPos;

            iTween.MoveTo(
                gameObject,
                iTween.Hash(
                    "x", FinalPos.x,
                    "y", FinalPos.y,
                    "easetype", "easeInOutQuad",
                    "time", EntryTime));  
        }
        startStill = false;
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
        if(!dontDisable)
		    transform.parent.gameObject.SetActive(false);
        else
        {
            transform.parent.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            dontDisable = false;
        }

	}
}
