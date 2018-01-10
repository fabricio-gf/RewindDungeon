using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPreview : MonoBehaviour {

    public void Init(int row, int col)
    {
        name = "Coin @ " + row + " " + col;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, Vector2.one);
    }
}
