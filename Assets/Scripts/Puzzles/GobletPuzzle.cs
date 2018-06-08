using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GobletPuzzle : MonoBehaviour {

    /// <summary>
    /// The list of sprites for each goblet. 
    /// </summary>
    public List<Sprite> gobletSprites;

    public delegate Sprite GetSprite(Goblet.Type t);
    /// <summary>
    /// Gets the sprite for a given goblet. 
    /// </summary>
    public static GetSprite SpriteEvent;

    private void Awake()
    {
        SpriteEvent = FindGobletSprite;
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
}
