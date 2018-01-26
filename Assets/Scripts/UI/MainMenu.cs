using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public MusicManager MM;
    public SoundManager SM;

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

    public void SetMusicVolume(float volume)
    {
        MuteMusic(false);
        MM.GetComponent<AudioSource>().volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        MuteSound(false);
        SM.GetComponent<AudioSource>().volume = volume;
    }

    public void MuteMusic(bool toggle)
    {
        MM.GetComponent<AudioSource>().mute = toggle;
        
    }

    public void MuteSound(bool toggle)
    {
        SM.GetComponent<AudioSource>().mute = toggle;
    }

    public void ChangeSpriteState(GameObject sprite)
    {
        sprite.SetActive(!sprite.activeSelf);
    }

    public void ChangeLanguage(string language)
    {
        if(language == "EN")
        {
            print("english");
        }
        else if(language == "PT")
        {
            print("portugues");
        }
        else
        {
            print("language not supported");
        }
    }

    public enum Language
    {
        PT,
        EN
    }
}
