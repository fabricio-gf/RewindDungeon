using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerClassMarker : MonoBehaviour {

	public Level.PlayerClass playerClass;
	public Color color;

	public void Init(Level.PlayerClass cls) {
		switch (cls) {
			case Level.PlayerClass.ARCHER:
				color = Color.green;
				break;
			case Level.PlayerClass.THIEF:
				color = Color.gray;
				break;
			case Level.PlayerClass.WARRIOR:
				color = Color.blue;
				break;
		}
		playerClass = cls;
		name = "character: " + playerClass;
	}

	void OnDrawGizmos() {
		Gizmos.color = color;
		Gizmos.DrawCube(transform.position, 0.5f * Vector2.one);
	}

}
