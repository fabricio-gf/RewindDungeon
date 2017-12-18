using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsoleteGameManager : MonoBehaviour {
	public static ObsoleteGameManager GM {
		get; private set;
	}

	void Awake() {
		if (GameObject.FindGameObjectsWithTag("GM").Length > 1) {
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			GM = GetComponent<ObsoleteGameManager>();
		}
	}

}
