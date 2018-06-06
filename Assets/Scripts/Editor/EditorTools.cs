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

    /*
    [MenuItem("Sins/Tools")]
    static void ToolsInit()
    {
        EditorWindow window = GetWindow(typeof(EditorTools));
        window.Show();
    }
    */
}