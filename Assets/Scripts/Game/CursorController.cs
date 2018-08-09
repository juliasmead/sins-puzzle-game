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
	public static Action<string> Click;

	private Animation anim;
	private Image im;

	void Awake()
	{
		anim = GetComponent<Animation>();
		im = GetComponent<Image>();
		Click = delegate (string s) { StartCoroutine(PlayClick(s)); };
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			anim.Stop();
			transform.position = Input.mousePosition;
			StartCoroutine(PlayClick("Click", false));
		}
	}

	private IEnumerator PlayClick(string s, bool delayed = true)
	{
		if(delayed)
		{
			yield return new WaitForSecondsRealtime(0.05f);
		}
		anim.Stop();
		im.color = new Color(0,0,0,0);
		transform.position = Input.mousePosition;
		if (anim.GetClip(s) != null)
		{
			anim.Play(s);
		}
		yield break;
	}
}
