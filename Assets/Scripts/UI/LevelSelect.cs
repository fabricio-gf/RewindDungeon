using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    public GameObject PreviewPanel;
	
	void LoadLevels(){
		//loads levels and information like is it locked, number of coins, thumbnail(?)
	}

    public void OpenPreview()
    {
        print(PreviewPanel.activeSelf);
        if (!PreviewPanel.activeSelf)
        {
            PreviewPanel.SetActive(true);
        }
    }

    public void LoadPreviewInfo()
    {
        print("loading");
    }

	public void ChangeScene(string str){
		SceneManager.LoadScene(str);
	}
}
