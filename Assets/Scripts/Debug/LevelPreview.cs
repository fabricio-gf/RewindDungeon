using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelPreview : MonoBehaviour {

	public GameObject debugObject;

	[Space(10)]
	public string levelName;

	void OnEnable() {
		Reset();
		Load();
	}

	public void Reset() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Debug")) {
			DestroyImmediate(obj);
		}
	}

	public void Load() {
		TextAsset ta = Resources.Load("Levels/" + levelName) as TextAsset;
		if (ta != null) {
			// TODO load level
			print(ta.ToString());
		} else {
			Debug.LogError("No such level: " + levelName);
		}
		GameObject testObj = Instantiate(
			debugObject,
			CenterPoint(3, 2),
			Quaternion.identity);
		testObj.AddComponent<EnemyPreview>();
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		for (int i = 0; i <= Board.GRID_ROWS; i++) {
			Vector2 from = CornerPoint(i, 0);
			Vector2 to = CornerPoint(i, Board.GRID_COLS);
			Gizmos.DrawLine(from, to);
		}
		for (int i = 0; i <= Board.GRID_COLS; i++) {
			Vector2 from = CornerPoint(0, i);
			Vector2 to = CornerPoint(Board.GRID_ROWS, i);
			Gizmos.DrawLine(from, to);
		}
	}

	Vector2 CornerPoint(int r, int c) {
		return new Vector2(c, -r);
	}

	Vector2 CenterPoint(int r, int c) {
		return new Vector2(c + 0.5f, -(r + 0.5f));
	}

}
