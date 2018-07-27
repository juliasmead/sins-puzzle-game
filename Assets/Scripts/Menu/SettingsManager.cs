using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the settings. 
/// </summary>
public class SettingsManager : MonoBehaviour
{

    [Header("Interactables")]
    /// <summary>
    /// The fullscreen toggle. 
    /// </summary>
    public Toggle fullscreen;
    /// <summary>
    /// The resolution dropdown.
    /// </summary>
    public TMP_Dropdown resolution;
    /// <summary>
    /// The master volume slider. 
    /// </summary>
    public Slider master;
    /// <summary>
    /// The music volume slider. 
    /// </summary>
    public Slider music;
    /// <summary>
    /// The soundfx volume slider. 
    /// </summary>
    public Slider soundfx;

    [Header("References")]
    /// <summary>
    /// Main audio mixer.
    /// </summary>
    public AudioMixer mix;

    void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(FileData.Save);

        fullscreen.onValueChanged.AddListener(delegate
        {
            FullToggle();
        });

        resolution.onValueChanged.AddListener(delegate
        {
            ResChange();
        });

        master.onValueChanged.AddListener(delegate
        {
            MasterChange();
        });

        music.onValueChanged.AddListener(delegate
        {
            MusicChange();
        });

        soundfx.onValueChanged.AddListener(delegate
        {
            SoundfxChange();
        });
    }

    public void OnEnable()
    {
        resolution.options = new List<TMP_Dropdown.OptionData>();
        List<string> resStrings = new List<string>();
        for (int i = Screen.resolutions.Length - 1; i >= 0; --i)
        {
            if (!resStrings.Contains(Screen.resolutions[i].ToString()))
            {
                resStrings.Add(Screen.resolutions[i].ToString());
            }
        }
        foreach (string r in resStrings)
        {
            resolution.options.Add(new TMP_Dropdown.OptionData(r));
        }
        fullscreen.isOn = Screen.fullScreen;
        resolution.value = (Screen.resolutions.Length - 1) - FileData.data.resolutionIndex;
        resolution.RefreshShownValue();
        master.value = FileData.data.master;
        music.value = FileData.data.music;
        soundfx.value = FileData.data.soundfx;
    }

    /// <summary>
    /// Toggles full screen mode.
    /// </summary>
    public void FullToggle()
    {
        Screen.fullScreen = FileData.data.fullscreen = fullscreen.isOn;
    }


    /// <summary>
    /// Changes the resolution.
    /// </summary>
    public void ResChange()
    {
        int val = (Screen.resolutions.Length - 1) - resolution.value;
        Screen.SetResolution(Screen.resolutions[val].width, Screen.resolutions[val].height,
            Screen.fullScreen);
        FileData.data.resolutionIndex = val;
    }

    /// <summary>
    /// Changes the master volume. 
    /// </summary>
    public void MasterChange()
    {
        FileData.data.master = master.value;
        mix.SetFloat("MasterVolume", FileData.data.master);
    }


    /// <summary>
    /// Changes the music volume. 
    /// </summary>
    public void MusicChange()
    {
        FileData.data.music = music.value;
        mix.SetFloat("MusicVolume", FileData.data.music);
    }


    /// <summary>
    /// Changes the sound fx volume. 
    /// </summary>
    public void SoundfxChange()
    {
        FileData.data.soundfx = soundfx.value;
        mix.SetFloat("SFXVolume", FileData.data.soundfx);
    }
}
