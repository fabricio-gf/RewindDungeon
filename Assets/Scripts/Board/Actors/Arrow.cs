using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Actor thisActor = GetComponent<Actor>();
		for(int i = 0; i < 8; i++)
        {
            //achar um jeito de mover sempre pra direção que o personagem estiver olhando
            thisActor.AddAction(Actor.Action.MOVE_R);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        //freeze position
        if(other.tag == "Player" || other.tag == "Enemy")
        {
            other.GetComponent<Actor>().TakeDamage();
        }
    }
}
