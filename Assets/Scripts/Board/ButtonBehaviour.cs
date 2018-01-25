using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour {

    public GameObject door;
    public int targetRow, targetCol;
    bool trigger = false;

    public void Trigger()
    {
        trigger = !trigger;

        GameManager.GM.board.Set(targetRow, targetCol, null);

        if(trigger)
            iTween.MoveBy(
                door,
                iTween.Hash(
                    "y", -1));
        else
            iTween.MoveBy(
                door,
                iTween.Hash(
                    "y", 1));
    }

    
}
