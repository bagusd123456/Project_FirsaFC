using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings_PanelUI : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        UpdateSliderStartValue();
    }

    public void MasterVolume()
    {
        AudioManager.Instance.SetMasterVolume(masterVolumeSlider);
    }

    public void MusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(musicVolumeSlider);
    }
    public void SfxVolume() { AudioManager.Instance.SetSFXVolume(sfxVolumeSlider);}

    public void UpdateSliderStartValue()
    {
        Debug.Log($"Update value to: {AudioManager.Instance.GetMasterVolume()}");
        masterVolumeSlider.value = AudioManager.Instance.GetMasterVolume();
        musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxVolumeSlider.value = AudioManager.Instance.GetSFXVolume();
    }

    public void SaveSettings()
    {
        AudioManager.Instance.SaveSettingsToJSON();
    }
}
