using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used for fading in and out transitions.
/// </summary>
public class Fader : MonoBehaviour
{
	public delegate void SelfFade(float f = 0.3f);
	public static SelfFade SelfFadeIn;
	public static SelfFade SelfFadeOut;

	public delegate IEnumerator Fade(float f = 0.3f);
	public static Fade FadeIn;
	public static Fade FadeOut;

	/// <summary>
	/// Fades in the given scene.
	/// </summary>
	public static Action<string> SceneEvent;

	private static Fader instance;

	private FadeableUI fadeable;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			fadeable = GetComponent<FadeableUI>();
			FadeIn = delegate (float f)
			{
				fadeable.Hide();
				return fadeable.FadeIn(dur: f);
			};
			FadeOut = delegate (float f)
			{
				fadeable.Show();
				return fadeable.FadeOut(dur: f);
			};
			SelfFadeIn = delegate (float f)
			{
				fadeable.Hide();
				fadeable.SelfFadeIn(dur: f);
			};
			SelfFadeOut = delegate (float f)
			{
				fadeable.Show();
				fadeable.SelfFadeOut(dur: f);
			};
			SceneEvent = delegate (string s) { 
				gameObject.SetActive(true);
				StartCoroutine(FadeInScene(s)); 
			};
		}
		else
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Fades in the given scene.
	/// </summary>
	private IEnumerator FadeInScene(string scene)
	{
		yield return fadeable.FadeIn();
		SceneManager.LoadScene(scene);
		yield return new WaitForSecondsRealtime(0.8f);
		yield return fadeable.FadeOut(dur: 0.5f);
	}
}
