using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// Main audio mixer.
	/// </summary>
	public AudioMixer mix;

	private void Awake()
	{
		Utils.Load(mix);
	}
}
