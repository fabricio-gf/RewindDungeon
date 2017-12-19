using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyPreview : MonoBehaviour {

	public List<Position> path;

	public void Init(Level.EnemyType type, Position start) {
		path.Add(start);
		name = "Enemy instance: " + type + " @ " + start.row + " " + start.col;
	}

	public void AddAction(Actor.Action action) {
		Position next = path[path.Count-1].Move(action);
		path.Add(next);
	}

	void OnEnable() {
		path = new List<Position>();
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position, Vector2.one);
	}

	void OnDrawGizmosSelected() {
		Color c = Color.red;
		Gizmos.color = c;
		for (int i = 1; i < path.Count; i++) {
			Gizmos.DrawLine(
				LevelPreview.CenterPoint(path[i-1]),
				LevelPreview.CenterPoint(path[i]));
		}
	}

}
