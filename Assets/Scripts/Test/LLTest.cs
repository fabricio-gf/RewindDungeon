using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LLTest : MonoBehaviour {
	void Start() {
		LevelManager lm = GameObject.FindObjectOfType<LevelManager>();
		lm.Load("T0");
	}
}
