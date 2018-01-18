using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSpawnPoint : RaycastTarget {

	public int r;
	public int c;

	override public void Click() {
        print("click");
        if(GameManager.GM.CharacterToSpawn != null)
        {
            Spawn(GameManager.GM.CharacterToSpawn);
        }
        else
        {
            GameManager.GM.selectedActor = null;
        }
	}

	public GameObject Spawn(GameObject characterPrefab) {
		GameObject obj = Instantiate(
			characterPrefab,
			GameManager.GM.board.GetCoordinates(r, c),
			characterPrefab.transform.rotation);
		Actor actor = obj.GetComponent<Actor>();
		actor.Spawn(GameManager.GM.board, r, c);
		GameManager.GM.actors.Add(actor);
        obj.GetComponent<PlayerCharacter>().SpawnPoint = this;
		gameObject.SetActive(false);
        GameManager.GM.selectedActor = actor;
        GameManager.GM.selectedButton.Available = false;
        GameManager.GM.selectedButton.UpdateUI();
        GameManager.GM.CharacterToSpawn = null;
        return obj;
	}

}
