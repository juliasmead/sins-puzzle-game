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
	[Range(0,20)]
	public float wallEdgeHeight;

	/// <summary>
	/// The width of the walls.
	/// </summary>
	private const float WALL_WIDTH = 7f;

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
		size = Vector2.one;
		oldSize = Vector2.one;
		oldBackgrounds = 0;
		oldEdgeHeight = 0;
	}

	void Update()
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

	private void RegenerateBackgrounds()
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
			sr.sortingLayerName = "Background";
			sr.sprite = s;
			SpriteUtility.ResizeSpriteRendererToDimensions(size, sr);
		}
		SRend.size = new Vector2((backgrounds + 0.2f) * size.x, size.y);
		Reposition();
		oldBackgrounds = backgrounds;
	}

	private Transform CreateWall(string n, float multiplier = 1, int sorting = 0)
	{
		GameObject g = new GameObject(n);
		g.transform.parent = transform;
		Material m = new Material(Shader.Find("Sprites/Default"));
		m.mainTexture = s.texture;
		MeshRenderer mr = g.AddComponent<MeshRenderer>();
		mr.sortingLayerName = "Background";
		mr.sortingOrder = sorting;
		mr.sharedMaterial = m;
		MeshFilter mf = g.AddComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mf.sharedMesh = mesh;

		Vector3[] vertices = new Vector3[4];

		vertices[0] = new Vector3(0, 0, 0);
		vertices[1] = new Vector3(WALL_WIDTH, size.y, 0);
		vertices[2] = new Vector3(0, size.y * multiplier, 0);
		vertices[3] = new Vector3(WALL_WIDTH, size.y * 2f, 0);

		mesh.vertices = vertices;

		int[] tri = new int[6];

		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;

		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;

		mesh.triangles = tri;

		Vector3[] normals = Enumerable.Repeat(-Vector3.forward, 4).ToArray();
		mesh.normals = normals;

		Vector2[] uv = new Vector2[4];

		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);

		mesh.uv = uv;

		if (multiplier == 1)
		{
			CreateWallEdge(g.transform);
		}

		return g.transform;
	}

	private void CreateWallEdge(Transform t) {
		GameObject g = new GameObject("Wall Edge");
		g.transform.parent = t;
		SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
		sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
		sr.color = new Color(0, 0, 0, 144f / 255f);
		sr.drawMode = SpriteDrawMode.Sliced;
		sr.size = new Vector2(WALL_EDGE_WIDTH, size.y);
		sr.sortingLayerName = "Background";
		sr.sortingOrder = 10;
		g.transform.localPosition = new Vector3(WALL_WIDTH + WALL_EDGE_ADJUST, (size.y * 1.5f) + wallEdgeHeight);
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
		Transform left = CreateWall("Left");
		Transform leftBack = CreateWall("LeftBack", 2, -5);
		Transform right = CreateWall("Right");
		Transform rightBack = CreateWall("LeftBack", 2, -5);
		right.localScale = new Vector3(-1, 1, 1);
		rightBack.localScale = right.localScale;
		left.localPosition = new Vector3(((size.x / -2f) * (backgrounds)) - WALL_WIDTH, size.y * -1.5f, 0);
		leftBack.localPosition = left.localPosition;
		right.localPosition = new Vector3(((size.x / 2f) * (backgrounds)) + WALL_WIDTH, size.y * -1.5f, 0);
		rightBack.localPosition = right.localPosition;
	}
#endif
}
