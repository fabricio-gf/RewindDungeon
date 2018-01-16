using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveActorSelection : RaycastTarget {

    public override void Click()
    {
        // TODO add way to deselect actor
        if (GameManager.GM.state == GameManager.State.PLANNING)
        {
            GameManager.GM.selectedActor = null;
        }
    }
}
