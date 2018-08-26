using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

/// <summary>
/// Editor tool for adding animation to an animator
/// </summary>
public class AddAnimationToAnimator : EditorWindow
{
	public AnimationClip animation;
	public AnimatorController animator;

	[MenuItem("Assets/Add Animation to Animator")]
	static void Init()
	{
		EditorWindow window = GetWindow(typeof(AddAnimationToAnimator));
		window.Show();
	}

	void OnGUI()
	{
		EditorGUILayout.Space();
		animation = (AnimationClip)EditorGUILayout.ObjectField("Animation", animation, typeof(AnimationClip), true);
		animator = (AnimatorController)EditorGUILayout.ObjectField("Animator", animator, typeof(AnimatorController), true);
		EditorGUILayout.Space();
		if (GUILayout.Button("Add Asset to path"))
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
			else
			{
				Debug.LogError("Assets must be assigned before adding.");
			}
		}
	}
}