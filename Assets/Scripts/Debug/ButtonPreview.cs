using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPreview : MonoBehaviour {

    public void Init(int row, int col)
    {
        name = "Button @ " + row + " " + col;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.41f, 0.13f, 0.55f);
        Gizmos.DrawCube(transform.position, Vector2.one);
    }
}
