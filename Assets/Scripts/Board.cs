using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Board : MonoBehaviour {

	public const float CELL_SIZE = 1.0f;
	public const int GRID_ROWS = 5;
	public const int GRID_COLS = 8;

	private GameObject[,] cells;

	void Awake() {
		cells = new GameObject[GRID_ROWS, GRID_COLS];
	}

	public GameObject Get(int r, int c) {
		return cells[r, c];
	}

	public void Set(int r, int c, GameObject obj) {
		cells[r, c] = obj;
	}

	public bool Move(int r0, int c0, int r1, int c1) {
		if (cells[r1, c1] != null) {
			return false;
		}
		cells[r1, c1] = cells[r0, c0];
		cells[r0, c0] = null;
		return true;
	}

}
