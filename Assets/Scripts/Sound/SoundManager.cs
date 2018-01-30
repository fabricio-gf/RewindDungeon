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
    public AudioClip bow;
    public AudioClip sword;
    public AudioClip lever;
    public AudioClip stairs;
    public AudioClip deathMale;
    public AudioClip deathFemale;
    public AudioClip deathWilhelm;
    public AudioClip[] bones = new AudioClip[3];

    System.Random rng;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("SM").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            SM = GetComponent<SoundManager>();
        }

        rng = new System.Random();
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
        yield return new WaitForSeconds(4.5f);
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

    public void BoneSound()
    {
        AudioClip clip = bones[rng.Next(bones.Length)];
        source.PlayOneShot(clip);
    }

    public void DeathMaleSound() {
        if (rng.Next(1000) < 1) {
            source.PlayOneShot(deathWilhelm);
        } else {
            source.PlayOneShot(deathMale);
        }
    }

    public void DeathFemaleSound() {
        source.PlayOneShot(deathFemale);
    }

    public void BowSound() {
        source.PlayOneShot(bow);
    }

    public void SwordSound() {
        source.PlayOneShot(sword);
    }
}
