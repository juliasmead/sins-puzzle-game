using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to create an auto-formatted repeatable sprite background. 
/// </summary>
[ExecuteInEditMode]
public class RepeatableBackground : MonoBehaviour
{
#if UNITY_EDITOR
	/// <summary>
	/// Sprite to be used for the background.
	/// </summary>
	public Sprite s;

	/// <summary>
	/// The number of backgrounds. 
	/// </summary>
	[Range(0,10)]
	public int backgrounds;

	/// <summary>
	/// The size of each background. 
	/// </summary>
	public Vector2 size;

	/// <summary>
	/// The old number of backgrounds.
	/// </summary>
	private int oldBackgrounds;

	/// <summary>
	/// The old size of the backgrounds. 
	/// </summary>
	private Vector2 oldSize;

	void Reset()
	{
		size = Vector2.one;
		oldSize = Vector2.one;
		oldBackgrounds = 0;
	}

	void Update()
	{
		if (oldBackgrounds != backgrounds)
		{
			int i = 0;
			while (i < transform.childCount)
			{
				DestroyImmediate(transform.GetChild(i).gameObject);
			}
			for (; i < backgrounds; ++i)
			{
				GameObject g = new GameObject("Bg " + i);
				g.transform.parent = transform;
				SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
				sr.sprite = s;
				SpriteUtility.ResizeSpriteRendererToDimensions(size, sr);
			}
			Reposition();
			oldBackgrounds = backgrounds;
		}
		if (oldSize != size)
		{
			foreach (Transform child in transform)
			{
				SpriteUtility.ResizeSpriteRendererToDimensions(size, child.GetComponent<SpriteRenderer>());
			}
			Reposition();
			oldSize = size;
		}
	}

	/// <summary>
	/// Repositions the children based on their count and size. 
	/// </summary>
	private void Reposition()
	{
		float x = (size.x / -2f) * (backgrounds - 1);
		foreach (Transform child in transform)
		{
			child.localPosition = new Vector3(x, 0, 0);
			x += size.x;
		}
	}
#endif
}
