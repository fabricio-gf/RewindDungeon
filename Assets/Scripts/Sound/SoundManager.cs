using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundManager))]
public class SoundManager : MonoBehaviour {

    public static SoundManager SM
    {
        get; private set;
    }

    public AudioSource source;

    public AudioClip button;
    public AudioClip panel;
    public AudioClip fanfare;
    public AudioClip lever;
    public AudioClip stairs;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("SM").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            SM = GetComponent<SoundManager>();
        }

        source = GetComponent<AudioSource>();
    }

    public void SetSoundVolume(float volume)
    {
        //MuteSound(false);
        source.volume = volume;
    }

    public void MuteSound(bool toggle)
    {
        source.mute = toggle;
    }

    public void ButtonSound()
    {
        source.PlayOneShot(button);
    }

    public void PanelSound()
    {
        source.PlayOneShot(panel);
    }

    public void FanfareSound()
    {
        MusicManager mm = MusicManager.MM;
        if (mm.source.volume > 0.7f && !mm.source.mute)
        {
            float vol = mm.source.volume;
            mm.source.volume = 0.7f;
            StartCoroutine(WaitToRaiseVolume(vol));
            StartCoroutine(PlayDelayed(fanfare, 0.5f));
        } else {
            source.PlayOneShot(fanfare);
        }
    }

    IEnumerator PlayDelayed(AudioClip clip, float delay) {
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(clip);

    }

    IEnumerator WaitToRaiseVolume(float vol)
    {
        yield return new WaitForSeconds(3f);
        MusicManager.MM.source.volume = vol;
    }

    public void LeverSound()
    {
        source.PlayOneShot(lever);
    }

    public void StairsSound()
    {
        source.PlayOneShot(stairs);
    }
}
