#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

static public class ProjectEditor
{
    public const string Menu = "EditorMenu/";

 
    [MenuItem(Menu + "RefreshCSVData", priority = -100)]
    static void RefreshCSVData()
    {
        
    }

}
#endif