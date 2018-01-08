using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public bool IsUnlocked = false;
    [Range (0,3)]
    public int Score = 0;

    GameObject LockedImage;
    GameObject[] CoinBacks;
    GameObject[] CoinFronts;

	// Use this for initialization
	void Awake () {
        LockedImage = transform.GetChild(1).gameObject;
        CoinBacks = new GameObject[3];
        CoinFronts = new GameObject[3];
        for(int i = 0; i < 3; i++)
        {
            CoinBacks[i] = transform.GetChild(i + 2).gameObject;
            CoinFronts[i] = transform.GetChild(i + 2).GetChild(0).gameObject;
        }
    }

    public void UpdateUI()
    {
        GetComponent<Button>().interactable = IsUnlocked;
        LockedImage.SetActive(!IsUnlocked);
        if (IsUnlocked)
        {
            for (int i = 0; i < 3; i++)
            {
                CoinBacks[i].SetActive(true);
                if(i < Score)
                {
                    CoinFronts[i].SetActive(true);
                }
            }
        }
    }

}
