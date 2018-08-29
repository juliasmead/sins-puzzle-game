using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the game's main UI. 
/// </summary>
public class UIManager : MonoBehaviour
{
	/// <summary>
	/// Assigns an action to clicking the right wall button. 
	/// </summary>
	public static Action<Action> AssignRightWallEvent;
	/// <summary>
	/// Assigns an action to clicking the left wall button. 
	/// </summary>
	public static Action<Action> AssignLeftWallEvent;

	/// <summary>
	/// Sets whether the right wall button is interactable. 
	/// </summary>
	public static Action<bool> SetRightWallInteractableEvent;
	/// <summary>
	/// Sets whether the left wall button is interactable. 
	/// </summary>
	public static Action<bool> SetLeftWallInteractableEvent;

	/// <summary>
	/// The right wall button.
	/// </summary>
	public Button rightWall;

	/// <summary>
	/// The left wall button.
	/// </summary>
	public Button leftWall;

	/// <summary>
	/// The pause button.
	/// </summary>
	public Button pauseButton;

	/// <summary>
	/// The pause screen UI.
	/// </summary>
	public FadeableUI pauseScreen;

	/// <summary>
	/// The pause screen resume button.
	/// </summary>
	public Button resumeButton;

	/// <summary>
	/// The pause screen menu button.
	/// </summary>
	public Button menuButton;

	/// <summary>
	/// The pause screen exit button.
	/// </summary>
	public Button exitButton;

	[System.NonSerialized]
	public static bool initialized = false;

	private void Awake()
	{
		leftWall.interactable = false;
		rightWall.interactable = false;
		AssignRightWallEvent = delegate (Action a) { AssignWallButton(rightWall, a); };
		AssignLeftWallEvent = delegate (Action a) { AssignWallButton(leftWall, a); };
		SetRightWallInteractableEvent = delegate (bool i) { SetWallInteractability(rightWall, i); };
		SetLeftWallInteractableEvent = delegate (bool i) { SetWallInteractability(leftWall, i); };
		initialized = true;
		pauseButton.onClick.AddListener(delegate {
			Time.timeScale = 0f;
			pauseScreen.SelfFadeIn(); 
		});
		resumeButton.onClick.AddListener(delegate {
			Time.timeScale = 1f;
			pauseScreen.SelfFadeOut(); 
		});
		menuButton.onClick.AddListener(delegate {
			Time.timeScale = 1f;
			Fader.SceneEvent("Menu", true); 
		});
		exitButton.onClick.AddListener(delegate { Utils.Exit(); });
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !pauseScreen.IsFading)
		{
			if (pauseScreen.IsVisible)
			{
				Time.timeScale = 1f;
				pauseScreen.SelfFadeOut();
			}
			else
			{
				Time.timeScale = 0f;
				pauseScreen.SelfFadeIn();
			}
		}
	}

	/// <summary>
	/// Assigns an action to clicking the given wall button. 
	/// </summary>
	private void AssignWallButton(Button wall, Action a)
	{
		wall.onClick.RemoveAllListeners();
		wall.onClick.AddListener(delegate { a(); });
	}

	/// <summary>
	/// Sets whether the given wall button is interactable. 
	/// </summary>
	private void SetWallInteractability(Button wall, bool i)
	{
		wall.interactable = i;
	}
}
