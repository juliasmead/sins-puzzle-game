using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor tools class
/// </summary>
public class EditorTools : EditorWindow
{
    
    public const string DOCUMENTATION_LINK = "https://drive.google.com/open?id=1ZLWWIlXvGnndTPRjVYy-1qfu2v-jb6cE";
    public const string GITHUB_LINK = "https://github.com/juliasmead/sins-puzzle-game";

    public const string HAX = "allUnlocks";
    public const string SAVE_PREFERENCES = "saveChoices";

    private bool hax = false;
    /// <summary>
    /// Should these preferences hold when the tools window is opened again?
    /// </summary>
    private bool savePreferences = false;


    [MenuItem("Sins/Documentation")]
    static void DocLink()
    {
        Application.OpenURL(DOCUMENTATION_LINK);
    }

    [MenuItem("Sins/GitHub")]
    static void GitLink()
    {
        Application.OpenURL(GITHUB_LINK);
    }

    [MenuItem("Sins/Tools")]
    static void ToolsInit()
    {
        EditorWindow window = GetWindow(typeof(EditorTools));
        window.Show();
    }

    void OnGUI()
    {
        savePreferences = EditorGUILayout.Toggle("Save Preferences", EditorPrefs.GetBool(SAVE_PREFERENCES));
        EditorPrefs.SetBool(SAVE_PREFERENCES, savePreferences);

        bool prefHax = savePreferences ? EditorPrefs.GetBool(HAX) : hax;

        hax = EditorGUILayout.Toggle("All Unlocks", prefHax);
        EditorPrefs.SetBool(HAX, hax);
    }
}