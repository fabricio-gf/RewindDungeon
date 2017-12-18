using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPreview : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, Vector2.one);
	}

}
