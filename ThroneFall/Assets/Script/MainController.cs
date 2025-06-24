using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

public class MainController : MonoBehaviour
{
    public static MainController Instance { get; private set; }
    public CSVDataContaner CSVDataContaner;
    //public InputManager inputManager;
    public ScriptableUnits ScriptableUnits;
    public ScriptableUnits ScriptableTowns;
    public GameObject sceneLoadScreen;
 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CSVDataContaner.Initialize();
        DontDestroyOnLoad(this);
        GameConfig.StartResolutionSetting();

    }


}
