using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public Sound(string name, AudioClip audioClip)
    {
        this.name = name;
        this.audioClip = audioClip;
    }

    [SerializeField]
    string name;
    public string Name { get { return name; } }

    [SerializeField]
    AudioClip audioClip;
    public AudioClip AudioClip { get { return audioClip; } }
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [SerializeField]
    List<Sound> sounds;

    [SerializeField] Transform sfxParent; // 여기 하위에 sfx 플레이어 넣어주기

    AudioSource bgmPlayer;
    AudioSource[] sfxPlayer;

    private void Awake()
    {
        instance = this;
        bgmPlayer = GetComponent<AudioSource>();
        sfxPlayer = sfxParent.GetComponentsInChildren<AudioSource>();
    }

    public void StopBGM()
    {
        if (bgmPlayer.isPlaying == true) bgmPlayer.Stop();
    }

    public void StopSFX()
    {
        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            if (sfxPlayer[i].isPlaying == false) continue;
            sfxPlayer[i].Stop();
        }
    }

    public void PlayBGM(string name, bool isLooping = false)
    {
        Sound sound = sounds.Find(x => x.Name == name);
        if (sound == null) return;

        StopBGM();

        bgmPlayer.clip = sound.AudioClip;
        bgmPlayer.loop = isLooping;

        bgmPlayer.Play();
    }

    public void PlaySFX(string name)
    {
        Sound sound = sounds.Find(x => x.Name == name);
        if (sound == null) return;

        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            if (sfxPlayer[i].isPlaying == true) continue;

            sfxPlayer[i].clip = sound.AudioClip;
            sfxPlayer[i].Play();
            break;
        }
    }

    public void SetBGMVolume(float ratio)
    {
        bgmPlayer.volume = ratio;
    }

    public void SetSFXVolume(float ratio)
    {
        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            sfxPlayer[i].volume = ratio;
        }
    }
}