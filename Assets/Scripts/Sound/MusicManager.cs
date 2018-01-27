using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MusicManager))]
public class MusicManager : MonoBehaviour {

    public static MusicManager MM
    {
        get; private set;
    }

    public AudioSource source;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("MM").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            MM = GetComponent<MusicManager>();
        }

        source = GetComponent<AudioSource>();
    }

    public void SetMusicVolume(float volume)
    {
        //MuteMusic(false);
        source.volume = volume;
    }

    public void MuteMusic(bool toggle)
    {
        source.mute = toggle;

    }
}
