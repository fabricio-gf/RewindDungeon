using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    [Header ("GAME LOADER")]
    [Tooltip ("Scriptable object that has references to the currently selected level")]
    public GameLoader Loader;

    [Header ("OBJECT REFERENCES")]
    [Tooltip ("Game object in the scene called PreviewPanel, which contains the information to be previewed")]
    public GameObject PreviewPanel;
    [Tooltip ("Name of the currently selected object")]
    public Text LevelText;
    [Tooltip("Image preview of the currently selected object")]
    public Image LevelImage;
    [Tooltip ("Object that parents the available classes icons")]
    public GameObject IconsLocation;

    [Space (20)]
    [Header ("PREFABS AND FILES")]
    [Tooltip ("Available class icon prefab")]
    public GameObject IconPrefab;

    [Tooltip ("Warrior icon sprite")]
    public Sprite WarriorIcon;
    [Tooltip ("Archer icon sprite")]
    public Sprite ArcherIcon;
    [Tooltip ("Thief icon sprite")]
    public Sprite ThiefIcon;

    //the player's currently selected level
    Level SelectedLevel = null;
    //a auxiliary list of available classes to a level
    List <Level.PlayerClass> Classes;

    /// <summary>
    /// If the player has not selected a level to preview yet, opens the preview window
    /// </summary>
    public void OpenPreview()
    {
        if (!PreviewPanel.activeSelf)
        {
            SoundManager.SM.PanelSound();
            PreviewPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Loads information about the selected level into the preview window, like name, image and available classes
    /// </summary>
    /// <param name="levelName"></param>
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
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = ArcherIcon;
                    break;
                case Level.PlayerClass.THIEF:
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = ThiefIcon;
                    break;
                case Level.PlayerClass.WARRIOR:
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = WarriorIcon;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Loads another scene by name
    /// </summary>
    /// <param name="str"></param>
	public void ChangeScene(string str){
		SceneManager.LoadScene(str);
	}

    public void SelectLevel()
    {
        Loader.SelectedLevel = SelectedLevel;
        ChangeScene("Cutscene");
    }

    public void PlayButtonSound()
    {
        SoundManager.SM.ButtonSound();
    }
}
