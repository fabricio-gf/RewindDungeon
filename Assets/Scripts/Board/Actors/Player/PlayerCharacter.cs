﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class PlayerCharacter : RaycastTarget {

	Actor actor;
    public PlayerSpawnPoint SpawnPoint;

	// Use this for initialization
	void Awake() {
		actor = GetComponent<Actor>();
	}

	public override void Click() {
        // TODO add way to deselect actor
        if (GameManager.GM.state == GameManager.State.PLANNING)
        {
            if (GameManager.GM.selectedActor != actor)
            {
                if(GameManager.GM.selectedActor != null)
                    GameManager.GM.selectedActor.transform.Find("Body").GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.clear);
                GameManager.GM.selectedActor = actor;
                GameManager.GM.selectedActor.transform.Find("Body").GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.red);
            }
            else
            {
                GameManager.GM.selectedActor.transform.Find("Body").GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.clear);
                GameManager.GM.selectedActor = null;
            }
        }
	}

    public void Remove()
    {
        SpawnPoint.gameObject.SetActive(true);
        Destroy(gameObject);
    }


}
