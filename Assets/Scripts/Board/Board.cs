using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public const float CELL_SIZE = 1.0f;
	public const int GRID_ROWS = 5;
	public const int GRID_COLS = 8;

	private GameObject[,] cells;

	public void Awake() {
		cells = new GameObject[GRID_ROWS, GRID_COLS];
	}

	public GameObject Get(int r, int c) {
		return cells[r, c];
	}

	public void Set(int r, int c, GameObject obj) {
		cells[r, c] = obj;
	}

	public bool Move(int r0, int c0, int r1, int c1) {
		if (!(r1 >= 0 && c1 >= 0
			&& r1 < cells.GetLength(0) && c1 < cells.GetLength(1))) {

			return false;
		}
		if (cells[r1, c1] != null) {
			return false;
		}
		cells[r1, c1] = cells[r0, c0];
		cells[r0, c0] = null;
		return true;
	}

	public void ClearActors() {
		for (int i = 0; i < cells.GetLength(0); i++) {
			for (int j = 0; j < cells.GetLength(1); j++) {
				GameObject obj = cells[i, j];
				if (obj != null && obj.GetComponent<Actor>() != null) {
					cells[i, j] = null;
				}
			}
		}
	}

	public Vector3 GetCoordinates(int r, int c) {
		// coordinates relative to top left corner
		float rx = 0.5f + c;
		float rz = 0.5f + r;

		// global coordinates
		float gx = rx - (GRID_COLS / 2f);
		float gz = -rz + (GRID_ROWS / 2f);
		// return new Vector3(gx, 0.5f, gz);
		return new Vector3(gx, 0, gz);
	}

    public bool WithinBounds(int r, int c)
    {
        return r >= 0 && r < GRID_ROWS && c >= 0 && c < GRID_COLS;
    }

}
