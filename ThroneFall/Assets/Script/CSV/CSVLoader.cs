using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public static class CSVLoader
{
    private static string CSVFilePath = Application.streamingAssetsPath;

    public static string[] Load(string fileName)
    {
        string path = Path.Combine(CSVFilePath, fileName);
        return File.ReadAllLines(path);
    }

}
