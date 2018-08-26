using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

/// <summary>
/// Editor tool for adding animations to an animator
/// </summary>
public class AddAnimationToAnimator : EditorWindow
{
	public List<AnimationClip> animations;
	public AnimatorController animator;

	[MenuItem("Assets/Add Animations to Animator")]
	static void Init()
	{
		EditorWindow window = GetWindow(typeof(AddAnimationToAnimator));
		window.Show();
	}

	void OnGUI()
	{
		if (animations == null)
		{
			animations = new List<AnimationClip>();
			animations.Add(null);
		}
		EditorGUILayout.Space();
		for (int i = 0; i < animations.Count; ++i)
		{
			animations[i] = (AnimationClip)EditorGUILayout.ObjectField("Animation " + i, animations[i], typeof(AnimationClip), true);
			if (animations[i] == null && i < animations.Count - 1)
			{
				animations.RemoveAt(i);
				--i;
			}
			else if (animations[i] != null && i == animations.Count - 1)
			{
				animations.Add(null);
			}
		}
		EditorGUILayout.Space();
		animator = (AnimatorController)EditorGUILayout.ObjectField("Animator", animator, typeof(AnimatorController), true);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (GUILayout.Button("Add Assets to path"))
		{
			if (animations.Count > 1 && animator != null)
			{
				foreach (AnimationClip animation in animations)
				{
					if (animation != null && animator != null)
					{
						AnimationClip a = Instantiate(animation);
						string n = animation.name;
						AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(animation));
						a.name = n;
						AssetDatabase.AddObjectToAsset(a, animator);
						AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animator));
						Debug.Log("Successfully added asset to path.");
					}
				}
			}
			else
			{
				Debug.LogError("Assets must be assigned before adding.");
			}
		}
	}
}