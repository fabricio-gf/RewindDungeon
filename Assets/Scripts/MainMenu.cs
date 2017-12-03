using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangePanel(string str)
    {
        //changes focus to another panel in this scene
    }

    public void OpenPanel(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void ClosePanel(GameObject obj)
    {
		obj.SetActive(false);
    }

    public void Teste()
    {
        print("teste");
    }
}
