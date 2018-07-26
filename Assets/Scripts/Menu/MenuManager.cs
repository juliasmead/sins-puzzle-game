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
    public GameObject main;
    public GameObject levelSelect;
    public GameObject credits;
    public GameObject settings;

    private void Awake()
    {
        Button[] mainButtons = main.GetComponentsInChildren<Button>();

        mainButtons[0].onClick.AddListener(StartGame);
        mainButtons[1].onClick.AddListener(Continue);
        mainButtons[2].onClick.AddListener(delegate { InvertPage(levelSelect); });
        mainButtons[3].onClick.AddListener(delegate { InvertPage(credits); });
        mainButtons[4].onClick.AddListener(delegate { InvertPage(settings); });
        mainButtons[5].onClick.AddListener(Exit);

        levelSelect.GetComponentInChildren<Button>().onClick.AddListener(delegate { InvertPage(levelSelect); });
        credits.GetComponentInChildren<Button>().onClick.AddListener(delegate { InvertPage(credits); });
        settings.GetComponentInChildren<Button>().onClick.AddListener(delegate { InvertPage(settings); });
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Lust");
    }

    private void Continue()
    {

    }

    private void InvertPage(GameObject g) {
        g.SetActive(!g.activeSelf);
        main.SetActive(!main.activeSelf);
    }

    private void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
