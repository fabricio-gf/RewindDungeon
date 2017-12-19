using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPreview : MonoBehaviour {

	public void Init(int row, int col) {
		name = "Wall @ " + row + " " + col;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawCube(transform.position, Vector2.one);
	}

}
