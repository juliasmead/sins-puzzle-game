using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Keeps track of all permanent data used across games.
/// </summary>
public static class FileData
{
    public static string[] sinsNames = new string[7] { "Lust", "Wrath", "Sloth", "Greed", "Gluttony", "Envy", "Pride" };

    /// <summary>
    /// For saving/loading data. 
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public int highestLevel = 0;
        public int currentLevel = 0;

        // Options variables
        public bool fullscreen;
        public int resolutionIndex;
        public float master;
        public float music;
        public float soundfx;

        public SaveData(AudioMixer mix)
        {
            fullscreen = Screen.fullScreen = true;
            for (int i = 0; i < Screen.resolutions.Length; ++i)
            {
                if (Screen.resolutions[i].Equals(Screen.currentResolution))
                {
                    resolutionIndex = i;
                }
            }
            mix.GetFloat("MasterVolume", out master);
            mix.GetFloat("MusicVolume", out music);
            mix.GetFloat("SFXVolume", out soundfx);
        }
    }

    public static SaveData data;

    // whether or not this has been loaded
    public static bool loaded;

    /// <summary>
    /// Unlocks all levels (for testing purposes).
    /// </summary>
    public static void Hax()
    {
        data.highestLevel = 7;
    }


    /// <summary>
    /// Saves the game.
    /// </summary>
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
#if UNITY_EDITOR
        FileStream file = File.Create(Application.persistentDataPath + "/sinsSavedInfoEditor" + ".zlb");
#else
        FileStream file = File.Create(Application.persistentDataPath + "/sinsSavedInfo" + ".zlb");
#endif
        bf.Serialize(file, data);
        file.Close();
    }


    /// <summary>
    /// Loads the game.
    /// </summary>
    /// <param name="mix">The main audio mixer.</param>
    public static void Load(AudioMixer mix)
    {
        bool fileFound = false;
#if UNITY_EDITOR
        if (File.Exists(Application.persistentDataPath + "/sinsSavedInfoEditor" + ".zlb"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/sinsSavedInfoEditor" + ".zlb", FileMode.Open);
#else
        if (File.Exists(Application.persistentDataPath + "/sinsSavedInfo" + ".zlb"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/sinsSavedInfo"+ ".zlb", FileMode.Open);
#endif
            try
            {
                data = (SaveData)bf.Deserialize(file);
                file.Close();

                Resolution[] resolutions = Screen.resolutions;

                data.resolutionIndex = Mathf.Min(data.resolutionIndex, Screen.resolutions.Length - 1);

                Screen.fullScreen = data.fullscreen;
                Screen.SetResolution(resolutions[data.resolutionIndex].width, resolutions[data.resolutionIndex].height,
                    Screen.fullScreen);
                mix.SetFloat("MasterVolume", data.master);
                mix.SetFloat("MusicVolume", data.music);
                mix.SetFloat("SFXVolume", data.soundfx);
                fileFound = true;
            }
            catch (SerializationException e)
            {
                Debug.LogWarning(e.Message + " Failed to deserialize save file.");
            }
        }
        if (!fileFound)
        {
            data = new SaveData(mix);
        }
        loaded = true;
    }
}