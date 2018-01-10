using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPreview : MonoBehaviour {

	public void Init (int row, int col) {
        name = "Exit @ " + row + " " + col;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, Vector2.one);
    }
}
