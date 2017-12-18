using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPreview : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position, Vector2.one);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, 1.125f);
	}

}
