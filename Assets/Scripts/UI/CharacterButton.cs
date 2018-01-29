using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour {

    public List<GameObject> CharPrefabs;
    public List<GameObject> CharPreviews;
    public List<Sprite> CharIcons;
    public bool Available = true;
    public int index;
    public Image UnavailableImage;

    public void Init() {
        transform.GetChild(0).GetComponent<Image>().sprite = CharIcons[index];
    }

    public void UpdateUI()
    {
        GetComponent<Button>().interactable = Available;
        //UnavailableImage.gameObject.SetActive(!Available);
    }

    public void SelectCharacter()
    {
        transform.parent.parent.GetComponent<PanelTransition>().ClosePanel();
        GameManager.GM.CharacterToSpawn = CharPrefabs[index];
        GameManager.GM.PreviewToSpawn = CharPreviews[index];
        GameManager.GM.selectedButton = this;
        Available = false;
        //alterações visuais
    }
}
