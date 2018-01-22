using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.GM.hasReachedExit = true;
            print(other.GetComponent<PlayerCharacter>().classType);
            other.GetComponent<Actor>().done = true;
        }
    }
}
