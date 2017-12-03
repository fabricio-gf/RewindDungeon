using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeOutQuad", "loopType", "loop", "delay", 0.1, "time", 0.5));
	}
}

