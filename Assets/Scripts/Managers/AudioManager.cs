using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public static Action<AudioClip> OnPlaySFX;

    [Header("Audio Properties")]
    public AudioMixer audioMixer;
    public AudioSource sfxSource;

    [Header("Audio Assets")]
    private AudioClip[] audioTarget;

    [Space(10)]
    [Header("SFX")]
    public AudioClip[] sfxClips;
    private void Awake()
    {
        audioTarget = Resources.LoadAll<AudioClip>("Uwu");
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void OnEnable()
    {
        OnPlaySFX += PlaySFX;
    }

    private void OnDisable()
    {
        OnPlaySFX -= PlaySFX;
    }

    /// <summary>
    /// Set the master volume of the game.
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(Slider volume)
    {
        audioMixer.SetFloat("masterVolume", volume.value);
    }

    /// <summary>
    /// Set the music volume of the game.
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(Slider volume)
    {
        audioMixer.SetFloat("bgmVolume", volume.value);
    }

    /// <summary>
    /// Set the SFX volume of the game.
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolume(Slider volume)
    {
        audioMixer.SetFloat("sfxVolume", volume.value);
    }

    /// <summary>
    /// Play the SFX clip.
    /// </summary>
    /// <param name="sfxClip"></param>
    public void PlaySFX(AudioClip sfxClip)
    {
        if(!sfxSource.isPlaying)
            sfxSource.PlayOneShot(sfxClip);
        else
        {
            sfxSource.Stop();
            sfxSource.PlayOneShot(sfxClip);
        }
    }

    /// <summary>
    /// I don't know what this is for.
    /// </summary>
    public void UwUTrigger()
    {
        AudioManager.OnPlaySFX?.Invoke(audioTarget[Random.Range(0, audioTarget.Length)]);
    }
}
