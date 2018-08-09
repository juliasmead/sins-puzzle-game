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

    private void Awake()
    {
        leftWall.interactable = false;
        rightWall.interactable = false;
        AssignRightWallEvent = delegate (Action a) { AssignWallButton(rightWall, a); };
        AssignLeftWallEvent = delegate (Action a) { AssignWallButton(leftWall, a); };
        SetRightWallInteractableEvent = delegate (bool i) { SetWallInteractability(rightWall, i); };
        SetLeftWallInteractableEvent = delegate (bool i) { SetWallInteractability(leftWall, i); };
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
