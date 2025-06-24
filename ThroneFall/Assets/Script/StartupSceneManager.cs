using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupSceneManager : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.StartLoadSceneAsync("TitleScene",null, true);
        SaveDataManager.Load();
    }
}
