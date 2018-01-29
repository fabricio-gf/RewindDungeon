using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveActorSelection : RaycastTarget {

    public override void Click()
    {
    	print("remove from " + name);
        if (GameManager.GM.state == GameManager.State.PLANNING)
        {
            GameManager.GM.selectedActor = null;
            GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>().DeactivateButtons();
        }
        
    }
}
