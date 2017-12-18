using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour {

    public List<GameObject> CharPrefabs;
    public int index;

    public void SelectCharacter()
    {
        transform.parent.parent.GetComponent<PanelTransition>().ClosePanel();
        GameManager.GM.CharacterToSpawn = CharPrefabs[index];
    }
}
