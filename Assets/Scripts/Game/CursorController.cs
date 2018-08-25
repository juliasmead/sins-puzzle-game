using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the cursor clicks. 
/// </summary>
public class CursorController : MonoBehaviour
{
	/// <summary>
	/// Gives the cursor a click. 
	/// </summary>
	public static Action<string> Click;
	/// <summary>
	/// Stops the cursor from generating a click. 
	/// </summary>
	public static Action NoClick;

	private Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
		Click = delegate (string s) { PlayClick(s); };
		NoClick = delegate { EliminateClick(); };
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			PlayClick("Click");
		}
	}

	/// <summary>
	/// Causes a click to play (animation set to the given string).
	/// </summary>
	/// <param name="s">S.</param>
	private void PlayClick(string s)
	{
		Vector3 pos = Input.mousePosition;
		pos.z = -Camera.main.transform.position.z;
		pos = Camera.main.ScreenToWorldPoint(pos);
		transform.position = pos;
		anim.SetTrigger(s);
	}

	/// <summary>
	/// Eliminates the click.
	/// </summary>
	private void EliminateClick()
	{
		anim.SetTrigger("None");
	}
}
