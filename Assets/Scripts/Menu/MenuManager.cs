using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// For managing the main menu. 
/// </summary>
public class MenuManager : MonoBehaviour
{

    public Button play;

    private void Awake()
    {
        play.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Lust");
    }
}
