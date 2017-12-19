using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LevelPreview))]
public class LevelPreviewLoader : MonoBehaviour {

	public GameObject prefabGM;
	LevelPreview preview;

	void Awake() {
		Instantiate(prefabGM);
		preview = GetComponent<LevelPreview>();
		DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded += SceneLoaded;
		GameManager.GM.Load(preview.levelName);
	}

	void SceneLoaded(Scene scene, LoadSceneMode mode) {
		SceneManager.sceneLoaded -= SceneLoaded;
		StartCoroutine(WaitTillLevelReady());
	}

	IEnumerator WaitTillLevelReady() {
		while (GameManager.GM.state != GameManager.State.PLANNING) {
			yield return new WaitForSeconds(0.25f);
		}
		if (preview.skipPlayerInput) {
			GameManager.GM.StartLoop();
		}
	}

}
