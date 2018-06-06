using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor documentation class
/// </summary>
public class EditorDocumentation
{
    #region CONSTANTS

    public const string DOCUMENTATION_LINK = "https://drive.google.com/open?id=1ZLWWIlXvGnndTPRjVYy-1qfu2v-jb6cE";

    #endregion


    [MenuItem("Sins/Documentation")]
    static void Init()
    {
        Application.OpenURL(DOCUMENTATION_LINK);
    }
}