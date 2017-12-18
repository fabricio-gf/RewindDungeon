﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {

    public List<GameObject> AvailableCharacters;

    public GameObject ButtonPrefab;

	// Use this for initialization
	void Start () {
        AvailableCharacters = GameManager.LM.playerAvailableCharactersPrefabs;
        GameObject obj;

        for(int i = 0; i < AvailableCharacters.Count; i++)
        {
            obj =  Instantiate(ButtonPrefab, transform);
            obj.transform.Find("Text").GetComponent<Text>().text = AvailableCharacters[i].GetComponent<Actor>().info.Preview.ToString();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}