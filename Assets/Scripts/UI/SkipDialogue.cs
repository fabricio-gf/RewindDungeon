using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipDialogue : MonoBehaviour {

    public GameLoader Loader;

    public void Skip()
    {
        GameManager.GM.Load(Loader.SelectedLevel.name);
    }

    public void PlayButtonSound()
    {
        SoundManager.SM.ButtonSound();
    }
}
