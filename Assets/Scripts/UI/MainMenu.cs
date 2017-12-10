using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void ChangeScene(string str)
    {
		SceneManager.LoadScene(str);
    }

    public void OpenPanel(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void ClosePanel(GameObject obj)
    {
		obj.transform.GetChild(1).GetComponent<PanelTransition>().ClosePanel();
    }

    public void Teste()
    {
        print("teste");
    }
}
