using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {

    public List<GameObject> AvailableCharacters;
    public List<GameObject> CharacterPreviews;
    public List<Sprite> CharacterIcons;

    public GameObject ButtonPrefab;

	public void Init() {
        AvailableCharacters = GameManager.GM.playerAvailableCharactersPrefabs;
        CharacterPreviews = GameManager.GM.playerPreviews;
        CharacterIcons = GameManager.GM.playerIcons;

        GameObject obj;
        for (int i = 0; i < AvailableCharacters.Count; i++)
        {
            obj = Instantiate(ButtonPrefab, transform);

            obj.GetComponent<CharacterButton>().index = i;
            obj.GetComponent<CharacterButton>().CharPrefabs = AvailableCharacters;
            obj.GetComponent<CharacterButton>().CharPreviews = CharacterPreviews;
            obj.GetComponent<CharacterButton>().CharIcons = CharacterIcons;

            obj.GetComponent<CharacterButton>().Init();
        }
    }
}
