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
    public delegate void AssignWall(Action a);
    /// <summary>
    /// Assigns an action to clicking the right wall button. 
    /// </summary>
    public static AssignWall AssignRightWallEvent;
    /// <summary>
    /// Assigns an action to clicking the left wall button. 
    /// </summary>
    public static AssignWall AssignLeftWallEvent;

    public delegate void SetWallInteractable(bool i);
    /// <summary>
    /// Sets whether the right wall button is interactable. 
    /// </summary>
    public static SetWallInteractable SetRightWallInteractableEvent;
    /// <summary>
    /// Sets whether the left wall button is interactable. 
    /// </summary>
    public static SetWallInteractable SetLeftWallInteractableEvent;

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
