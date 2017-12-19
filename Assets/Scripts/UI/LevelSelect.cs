using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    public GameObject PreviewPanel;

    public GameObject IconPrefab;

    public Sprite WarriorIcon;
    public Sprite ArcherIcon;
    public Sprite ThiefIcon;

    Level SelectedLevel = null;
    public Text LevelText;
    public Image LevelImage;
    public GameObject IconsLocation;
    List <Level.PlayerClass> Classes;

    void LoadLevels(){
		//loads levels and information like is it locked, number of coins, thumbnail(?)
	}

    public void OpenPreview()
    {
        if (!PreviewPanel.activeSelf)
        {
            PreviewPanel.SetActive(true);
        }
    }

    public void LoadPreviewInfo(string name)
    {
        if(SelectedLevel != null)
        {
            var children = new List<GameObject>();
            foreach (Transform child in IconsLocation.transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        SelectedLevel = Resources.Load<Level>("Levels/" + name);

        LevelText.text = SelectedLevel.title;
        LevelImage.sprite = Resources.Load<Sprite>("Previews/" + SelectedLevel.spritePath);
        Classes = SelectedLevel.classes;
        for (int i = 0; i < Classes.Count; i++)
        {
            GameObject obj = Instantiate(IconPrefab, IconsLocation.transform);
            switch (Classes[i]) {
                case Level.PlayerClass.ARCHER:
                    obj.GetComponent<Image>().sprite = ArcherIcon;
                    break;
                case Level.PlayerClass.THIEF:
                    obj.GetComponent<Image>().sprite = ThiefIcon;
                    break;
                case Level.PlayerClass.WARRIOR:
                    obj.GetComponent<Image>().sprite = WarriorIcon;
                    break;
                default:
                    break;
            }
        }
    }

	public void ChangeScene(string str){
		SceneManager.LoadScene(str);
	}
}
