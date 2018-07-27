using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the level select screen.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    private void Awake()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(delegate { LoadLevel(b.GetComponent<TextMeshProUGUI>().text); });
        }
    }

    private void LoadLevel(string s)
    {
        SceneManager.LoadScene(s);
    }
}
