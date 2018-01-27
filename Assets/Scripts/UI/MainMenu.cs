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

    public void SetMusicVolume(float volume)
    {
        //MuteMusic(false);
        MusicManager.MM.SetMusicVolume(volume);
    }

    public void SetSoundVolume(float volume)
    {
        //MuteSound(false);
        SoundManager.SM.SetSoundVolume(volume);
    }

    public void MuteMusic(bool toggle)
    {
        MusicManager.MM.MuteMusic(toggle);        
    }

    public void MuteSound(bool toggle)
    {
        SoundManager.SM.MuteSound(toggle);
    }

    public void ChangeSpriteState(GameObject sprite)
    {
        sprite.SetActive(!sprite.activeSelf);
    }

    public void PlayButtonSound()
    {
        SoundManager.SM.ButtonSound();
    }

    public void PlayPanelSound()
    {
        SoundManager.SM.PanelSound();
    }

}
