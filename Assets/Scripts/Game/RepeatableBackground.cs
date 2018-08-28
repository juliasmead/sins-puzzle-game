using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
	[Range(0, 10)]
	public int backgrounds;

	/// <summary>
	/// The size of each background. 
	/// </summary>
	public Vector2 size;

	/// <summary>
	/// The height off the ground the wall edge is. 
	/// </summary>
	[Range(0, 20)]
	public float wallEdgeHeight;

	/// <summary>
	/// The width of the walls.
	/// </summary>
	private const float WALL_WIDTH = 7f;

	/// <summary>
	/// The color of the walls.
	/// </summary>
	private const float WALL_COLOR = 230f / 255f;

	/// <summary>
	/// The width of the wall edges.
	/// </summary>
	private const float WALL_EDGE_WIDTH = 0.05f;

	/// <summary>
	/// The adjustment increment of the wall edges.
	/// </summary>
	private const float WALL_EDGE_ADJUST = 0.01f;

	/// <summary>
	/// The sprite renderer. 
	/// </summary>
	private SpriteRenderer sRend;

	private SpriteRenderer SRend
	{
		get
		{
			if (sRend == null)
			{
				sRend = GetComponent<SpriteRenderer>();
			}
			return sRend;
		}
	}

	/// <summary>
	/// The old number of backgrounds.
	/// </summary>
	private int oldBackgrounds;

	/// <summary>
	/// The old size of the backgrounds. 
	/// </summary>
	private Vector2 oldSize;

	/// <summary>
	/// The old height of the edge.
	/// </summary>
	private float oldEdgeHeight;

	void Reset()
	{
		if (!Application.isPlaying)
		{
			size = Vector2.one;
			oldSize = Vector2.one;
			oldBackgrounds = 0;
			oldEdgeHeight = 0;
		}
	}

	void Update()
	{
		if (!Application.isPlaying)
		{
			if (oldSize != size || oldEdgeHeight != wallEdgeHeight)
			{
				foreach (Transform child in transform)
				{
					SpriteUtility.ResizeSpriteRendererToDimensions(size, child.GetComponent<SpriteRenderer>());
				}
				oldSize = size;
				oldEdgeHeight = wallEdgeHeight;
				RegenerateBackgrounds();
			}
			else if (oldBackgrounds != backgrounds)
			{
				RegenerateBackgrounds();
			}
		}
	}

	private void RegenerateBackgrounds()
	{
		int i = 0;
		while (i < transform.childCount)
		{
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
		for (; i < backgrounds + 2; ++i)
		{
			GameObject g = new GameObject("Bg " + i);
			g.transform.parent = transform;
			SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
			sr.sortingLayerName = "Background";
			sr.sprite = s;
			SpriteUtility.ResizeSpriteRendererToDimensions(size, sr);
		}
		SRend.size = new Vector2((backgrounds + 0.2f) * size.x, size.y);
		Reposition();
		oldBackgrounds = backgrounds;
	}

	private void CreateWall(SpriteRenderer sr, bool flip = false)
	{
		sr.sprite = Sprite.Create(s.texture, s.rect, new Vector2(0.5f, 0.5f), s.pixelsPerUnit);
		SpriteUtility.ResizeSpriteRendererToDimensions(new Vector2(WALL_WIDTH, size.y), sr);
		Vector3 newPos = sr.transform.localPosition;
		newPos.x += (size.x - WALL_WIDTH) / (flip ? -2 : 2);
		sr.transform.localPosition = newPos;
		Shader sh = Shader.Find("Walls");
		sr.sharedMaterial = new Material(sh);
		sr.sharedMaterial.SetFloat("_Left", flip ? 0 : 1);
		sr.sharedMaterial.SetFloat("_Right", flip ? 1 : 0);
		sr.color = new Color(WALL_COLOR, WALL_COLOR, WALL_COLOR, 1);

		if (!flip)
		{
			Vector2[] sv = sr.sprite.vertices;
			ushort[] tri = sr.sprite.triangles;

			for (int i = 0; i < sv.Length; i++)
			{
				sv[i].x = Mathf.Clamp(
						(s.vertices[i].x - s.bounds.center.x -
						 (s.textureRectOffset.x / s.texture.width) + s.bounds.extents.x) /
						(2.0f * s.bounds.extents.x) * s.rect.width,
						0.0f, s.rect.width);

				sv[i].y = Mathf.Clamp(
						(s.vertices[i].y - s.bounds.center.y -
						 (s.textureRectOffset.y / s.texture.height) + s.bounds.extents.y) /
						(2.0f * s.bounds.extents.y) * s.rect.height,
						0.0f, s.rect.height);
			}

			tri[0] = 3;
			tri[1] = 2;
			tri[2] = 1;

			tri[3] = 2;
			tri[4] = 3;
			tri[5] = 0;

			sr.sprite.OverrideGeometry(sv, tri);
		}

		CreateWallEdge(sr.transform);
	}

	private void CreateWallEdge(Transform t)
	{
		GameObject g = new GameObject("Wall Edge");
		g.transform.parent = t.parent;
		SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
		sr.sprite = transform.GetComponent<SpriteRenderer>().sprite;
		sr.color = new Color(0, 0, 0, 144f / 255f);
		sr.drawMode = SpriteDrawMode.Sliced;
		sr.size = new Vector2(WALL_EDGE_WIDTH, size.y);
		sr.sortingLayerName = "Background";
		sr.sortingOrder = 10;
		g.transform.localPosition = new Vector3((((WALL_WIDTH / 2f) + WALL_EDGE_ADJUST) * 
		                                         -(t.transform.localPosition.x / Mathf.Abs(t.transform.localPosition.x))) 
		                                        + t.transform.localPosition.x, wallEdgeHeight);
		g.transform.parent = t;
	}

	/// <summary>
	/// Repositions the children based on their count and size. 
	/// </summary>
	private void Reposition()
	{
		float x = (size.x / -2f) * (backgrounds + 1);
		foreach (Transform child in transform)
		{
			child.localPosition = new Vector3(x, 0, 0);
			x += size.x;
		}
		CreateWall(transform.GetChild(0).GetComponent<SpriteRenderer>());
		CreateWall(transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>(), true);
	}
#endif
}
