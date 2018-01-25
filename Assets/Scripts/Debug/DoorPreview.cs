using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPreview : MonoBehaviour {

    public void Init(int row, int col)
    {
        name = "Door @ " + row + " " + col;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.45f, 0);
        Gizmos.DrawCube(transform.position, Vector2.one);
    }
}
