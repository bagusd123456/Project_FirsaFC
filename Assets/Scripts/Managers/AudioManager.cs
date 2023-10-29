using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioSettings audioSettings;
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

    private void Start()
    {
        AssignSettingsFromData();
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

    public float GetMasterVolume()
    {
        audioMixer.GetFloat("masterVolume", out float value);
        return value;
        //return audioMixer.GetFloat("masterVolume", out float value) ? value : 0;
    }

    public float GetMusicVolume()
    {
        audioMixer.GetFloat("bgmVolume", out float value);
        return value;
    }

    public float GetSFXVolume()
    {
        audioMixer.GetFloat("sfxVolume", out float value);
        return value;
    }

    public void SaveSettingsToJSON()
    {
        var data = new AudioSettings
        {
            masterVolume = GetMasterVolume(),
            musicVolume = GetMusicVolume(),
            sfxVolume = GetSFXVolume()
        };

        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("AudioSettings", json);
    }

    public AudioSettings LoadSettingsFromJSON()
    {
        var defaultData = new AudioSettings()
        {
            masterVolume = 0.5f,
            musicVolume = 0.5f,
            sfxVolume = 0.5f
        };

        string defaultDataJSON = JsonUtility.ToJson(defaultData, true);
        var data = JsonUtility.FromJson<AudioSettings>(PlayerPrefs.GetString("AudioSettings", defaultDataJSON));

        return data;
    }

    public void AssignSettingsFromData()
    {
        float master = LoadSettingsFromJSON().masterVolume;
        float bgm = LoadSettingsFromJSON().musicVolume;
        float sfx = LoadSettingsFromJSON().sfxVolume;
        audioMixer.SetFloat("bgmVolume", master);
        audioMixer.SetFloat("bgmVolume", bgm);
        audioMixer.SetFloat("sfxVolume", sfx);
    }

    /// <summary>
    /// I don't know what this is for.
    /// </summary>
    public void UwUTrigger()
    {
        AudioManager.OnPlaySFX?.Invoke(audioTarget[Random.Range(0, audioTarget.Length)]);
    }
}

public class AudioSettings
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}
