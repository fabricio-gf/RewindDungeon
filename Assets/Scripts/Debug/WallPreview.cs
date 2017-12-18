using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPreview : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawCube(transform.position, Vector2.one);
	}

}
