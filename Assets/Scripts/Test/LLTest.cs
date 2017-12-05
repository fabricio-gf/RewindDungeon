using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LLTest : MonoBehaviour {
	void Start() {
		LevelManager lm = GetComponent<LevelManager>();
		lm.Load("0");
	}
}
