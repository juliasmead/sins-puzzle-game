using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GobletPuzzle : MonoBehaviour
{
    /// <summary>
    /// Attempts to solve the goblet puzzle. 
    /// </summary>
    public static Action SolveEvent;

    /// <summary>
    /// The list of sprites for each goblet. 
    /// </summary>
    public List<Sprite> gobletSprites;

    public delegate Sprite GetSprite(Goblet.Type t);
    /// <summary>
    /// Gets the sprite for a given goblet. 
    /// </summary>
    public static GetSprite SpriteEvent;

    /// <summary>
    /// The list of all the goblets. 
    /// </summary>
    private Goblet[] goblets;

    private void Awake()
    {
        SolveEvent = Solve;
        SpriteEvent = FindGobletSprite;
        goblets = GetComponentsInChildren<Goblet>();
    }

    /// <summary>
    /// Finds the sprite for the given goblet. 
    /// </summary>
    /// <returns>The goblet sprite.</returns>
    private Sprite FindGobletSprite(Goblet.Type t)
    {
        if ((int)t < gobletSprites.Count && t != Goblet.Type.none)
        {
            return gobletSprites[(int)t];
        }
        return null;
    }

    /// <summary>
    /// Attempts to solve the goblet puzzle. 
    /// </summary>
    private void Solve()
    {
        if (Solved())
        {
            print("solved");
        }
    }

    /// <summary>
    /// Determines whether or not the goblet puzzle has been solved. 
    /// </summary>
    private bool Solved()
    {
        for (int i = 0; i < goblets.Length; ++i)
        {
            if ((int)goblets[i].AssignableType != i)
            {
                return false;
            }
        }
        return true;
    }
}
