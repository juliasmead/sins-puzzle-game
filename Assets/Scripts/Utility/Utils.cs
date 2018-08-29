using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// General utility class. 
/// </summary>
public static class Utils
{
	private const string HAX = "allUnlocks";

	public static void Load(AudioMixer mix)
	{
		if (!FileData.loaded)
		{
			FileData.Load(mix);
#if UNITY_EDITOR

			if (EditorPrefs.GetBool(HAX))
			{
				FileData.Hax();
			}
		}
#endif
	}

	/// <summary>
	/// Exits the game. 
	/// </summary>
	public static void Exit()
	{
		FileData.Save();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
}
