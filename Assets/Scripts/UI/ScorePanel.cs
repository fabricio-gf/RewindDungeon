using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour {

    public ScoreManager scoreManager;

    GameObject[] CoinFronts;

    private void Awake()
    {
        CoinFronts = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            CoinFronts[i] = transform.GetChild(i).GetChild(0).gameObject;
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < scoreManager.CurrentScore; i++)
        {
            CoinFronts[i].SetActive(true);
        }
    }
}
