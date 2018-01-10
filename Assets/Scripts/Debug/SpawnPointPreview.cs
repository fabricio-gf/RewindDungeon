using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPreview : MonoBehaviour {

	public void Init(int row, int col) {
		name = "Spawn point @ " + row + " " + col;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position, Vector2.one);
	}

}
