using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// For managing the main menu. 
/// </summary>
public class MenuManager : MonoBehaviour
{
	public GameObject main;
	public GameObject levelSelect;
	public GameObject credits;
	public GameObject settings;

	/// <summary>
	/// Main audio mixer.
	/// </summary>
	public AudioMixer mix;

	private const string HAX = "allUnlocks";

	private void Awake()
	{
		FileData.Load(mix);
#if UNITY_EDITOR
		if (EditorPrefs.GetBool(HAX))
		{
			FileData.Hax();
		}
#endif
		Button[] mainButtons = main.GetComponentsInChildren<Button>();

		mainButtons[0].onClick.AddListener(StartGame);
		mainButtons[1].onClick.AddListener(Continue);
		mainButtons[2].onClick.AddListener(delegate { InvertPage(levelSelect); });
		mainButtons[3].onClick.AddListener(delegate { InvertPage(credits); });
		mainButtons[4].onClick.AddListener(delegate { InvertPage(settings); });
		mainButtons[5].onClick.AddListener(Exit);

		levelSelect.GetComponentInChildren<Button>().onClick.AddListener(delegate { InvertPage(levelSelect); });
		credits.GetComponentInChildren<Button>().onClick.AddListener(delegate { InvertPage(credits); });
		settings.GetComponentInChildren<Button>().onClick.AddListener(delegate { InvertPage(settings); });
	}

	private void Start()
	{
		Fader.SelfFadeOut(1f);
	}

	private void StartGame()
	{
		// Final Version:
		//Fader.SceneEvent(FileData.sinsNames[0]);
		Fader.SceneEvent("Gluttony");
	}

	private void Continue()
	{
		Fader.SceneEvent(FileData.sinsNames[FileData.data.currentLevel]);
	}

	private void InvertPage(GameObject g)
	{
		g.SetActive(!g.activeSelf);
		main.SetActive(!main.activeSelf);
	}

	private void Exit()
	{
		FileData.Save();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
}
