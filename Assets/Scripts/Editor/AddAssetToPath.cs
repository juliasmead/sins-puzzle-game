using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor tool for adding assets to a given object. 
/// </summary>
public class AddAssetToPath : EditorWindow
{
	public Object assetToAdd;
	public Object assetAddedTo;

	[MenuItem("Assets/Add Asset To Path")]
	static void Init()
	{
		EditorWindow window = GetWindow(typeof(AddAssetToPath));
		window.Show();
	}

	void OnGUI()
	{
		EditorGUILayout.Space();
		assetToAdd = EditorGUILayout.ObjectField("Asset to add", assetToAdd, typeof(Object), true);
		assetAddedTo = EditorGUILayout.ObjectField("Asset added to", assetAddedTo, typeof(Object), true);
		EditorGUILayout.Space();
		if (GUILayout.Button("Add Asset to path"))
		{
			if (assetToAdd != null && assetAddedTo != null)
			{
				AssetDatabase.AddObjectToAsset(assetToAdd, assetAddedTo);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(assetAddedTo));
				Debug.Log("Successfully added asset to path.");
			}
			else
			{
				Debug.LogError("Assets must be assigned before adding.");
			}
		}
	}
}
