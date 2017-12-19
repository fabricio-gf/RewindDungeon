using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelPreview : MonoBehaviour {

	public GameObject previewObject;

	[Space(10)]
	public bool skipPlayerInput = true;
	public string levelName;

	void OnEnable() {
		Reset();
		Load();
	}

	public void Reset() {
		foreach (var obj in GameObject.FindGameObjectsWithTag("Debug")) {
			DestroyImmediate(obj);
		}
	}

	public void Load() {
		Level level = Resources.Load("Levels/" + levelName) as Level;
		if (level != null) {
			for (int i = 0; i < level.classes.Count; i++) {
				PlayerClassMarker marker =
					AddLevelObject<PlayerClassMarker>(-1, i);
				marker.Init(level.classes[i]);
			}
			foreach (Position wallPos in level.walls) {
				WallPreview wall = AddLevelObject<WallPreview>(
					wallPos.row, wallPos.col);
				wall.Init(wallPos.row, wallPos.col);
			}
			foreach (Position spawnPos in level.spawnPoints) {
				SpawnPointPreview sp = AddLevelObject<SpawnPointPreview>(
					spawnPos.row, spawnPos.col);
				sp.Init(spawnPos.row, spawnPos.col);
			}
			foreach (Level.EnemyInstance inst in level.enemies) {
				EnemyPreview enemy = AddLevelObject<EnemyPreview>(
					inst.position.row, inst.position.col);
				enemy.Init(inst.enemyType, inst.position);
				inst.plan.ForEach(
					action => enemy.AddAction(action));
			}
		} else {
			Debug.LogError("No such level: " + levelName);
		}
	}

	T AddLevelObject<T>(int r, int c) where T : Component {
		GameObject obj = Instantiate(previewObject);
		obj.transform.position = CenterPoint(r, c);
		return obj.AddComponent<T>();
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

	public static Vector2 CornerPoint(int r, int c) {
		return new Vector2(c, -r);
	}

	public static Vector2 CenterPoint(Position pos) {
		return CenterPoint(pos.row, pos.col);
	}

	public static Vector2 CenterPoint(int r, int c) {
		return new Vector2(c + 0.5f, -(r + 0.5f));
	}

}
