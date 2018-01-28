using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Actor))]
public class PlayerCharacter : RaycastTarget {

    public enum Class
    {
        ARCHER,
        THIEF,
        WARRIOR
    }

    public Class classType;

	Actor actor;
    public PlayerSpawnPoint SpawnPoint;

	// Use this for initialization
	void Awake() {
		actor = GetComponent<Actor>();
        if(classType == Class.THIEF)
        {
            actor.maxActions = 10;
        }
        else{
            actor.maxActions = 6;
            if (classType == Class.ARCHER)
            {
                actor.isArcher = true;
            }
        }
	}

	public override void Click() {

        // TODO add way to deselect actor
        if (GameManager.GM.state == GameManager.State.PLANNING)
        {
            if (GameManager.GM.selectedActor != actor)
            {
                GameManager.GM.selectedActor = actor;
                GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>().ActivateButtons();
            }
            else
            {
                GameManager.GM.selectedActor = null;
                GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>().DeactivateButtons();
            }
        }
        
	}

    public void Remove()
    {
        SpawnPoint.gameObject.SetActive(true);
        GameManager.GM.actors.Remove(actor);
        Destroy(gameObject);
    }
}
