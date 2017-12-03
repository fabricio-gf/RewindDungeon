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

    public void OpenPanel(string str)
    {
        //opens a smaller panel and does not change focus from the current panel
    }

    public void ClosePanel(string str)
    {

    }

    public void Teste()
    {
        print("teste");
    }
}
