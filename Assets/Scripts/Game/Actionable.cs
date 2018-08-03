using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object that will do an action. 
/// </summary>
[System.Serializable]
public abstract class Actionable : MonoBehaviour
{
	public abstract void ExecuteAction();
}
