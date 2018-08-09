using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For managing individual goblets. 
/// </summary>
[ExecuteInEditMode]
public class Goblet : MonoBehaviour
{
    public enum Type
    {
        none = -1,
        deadGold = 0,
        silver = 1,
        vineClear = 2,
        roseGold = 3,
        vineShortSilver = 4,
        gold = 5,
        birdSilver = 6,
        birdClear = 7,
        deadClear = 8, // The Crystal Skull!
    }

    public Type AssignableType
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            rend.sprite = GobletPuzzle.SpriteEvent(type);
        }
    }

    [SerializeField]
    private Type type;

    /// <summary>
    /// The sprite renderer
    /// </summary>
    [SerializeField]
    protected SpriteRenderer rend;

    /// <summary>
    /// Used for changing this goblet's type in editor. 
    /// </summary>
    private Type lastType = Type.none;

    /// <summary>
    /// The current goblet that is being hovered over.
    /// </summary>
    private static Goblet currentHover;

    /// <summary>
    /// This item's position before it followed the cursor. 
    /// </summary>
    private Vector3 originalPosition;

    /// <summary>
    /// Whether or not this item is currently following the cursor. 
    /// </summary>
    private bool following = false;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            if (following && Input.GetMouseButtonUp(0))
            {
                following = false;
            }
        }
        else
        {
            if (lastType != type)
            {
                lastType = type;
                rend.sprite = GobletPuzzle.SpriteEvent(type);
            }
        }
    }

    protected virtual void Reset()
    {
        rend = GetComponent<SpriteRenderer>();
        if (rend == null)
        {
            rend = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Follows the cursor after a brief delay. 
    /// </summary>
    private IEnumerator FollowCursor()
    {
        originalPosition = transform.position;
        following = true;
        col.enabled = false;
        rend.sortingOrder = 1;
        while (following)
        {
            Vector3 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(cursor.x, cursor.y, originalPosition.z);

            yield return new WaitForEndOfFrame();
        }
        transform.position = originalPosition;
        if (currentHover != this && currentHover != null)
        {
            Type tempType = type;
            AssignableType = currentHover.type;
            currentHover.AssignableType = tempType;
            GobletPuzzle.SolveEvent();
        }
        col.enabled = true;
        rend.sortingOrder = 0;
    }

    private void OnMouseDown()
    {
		CursorController.Click("");
        StartCoroutine(FollowCursor());
    }

    private void OnMouseEnter()
    {
        currentHover = this;
    }

    private void OnMouseExit()
    {
        if (currentHover == this)
        {
            currentHover = null;
        }
    }
}
