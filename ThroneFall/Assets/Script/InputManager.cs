using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class InputManager : MonoBehaviour
{
    static List<IInputSubscriber> _inputSubscribers = new List<IInputSubscriber>();
    public List<InputBinding> bindings = new List<InputBinding>();
    private Dictionary<KeyCode, float> _pressDurations = new Dictionary<KeyCode, float>();

    private void Start()
    {
        if (PlayerPrefs.GetInt("FirstBinding") == 0)
        {
            FirstKeyBinding();
        }

        SaveDataManager.LoadSettings();
        bindings = SaveDataManager.SaveSettingData.InputSetting._inputBindings;
    }

    public void FirstKeyBinding()
    {
        InputSettings inputSettings = new();
        inputSettings._inputBindings.Add(new InputBinding()
        {
            actionName = "Move",
            key = KeyCode.Mouse1
        });
        inputSettings._inputBindings.Add(new InputBinding()
        {
            actionName = "Interaction",
            key = KeyCode.Mouse0
        });
        inputSettings._inputBindings.Add(new InputBinding()
        {
            actionName = "Cancel",
            key = KeyCode.Escape
        });
        inputSettings._inputBindings.Add(new InputBinding()
        {
            actionName = "CallUp",
            key = KeyCode.R
        });
        
        SaveDataManager.SetInputSetting(inputSettings);
        SaveDataManager.SaveSettings();
        PlayerPrefs.SetInt("FirstBinding",1);
    }

    private void Update()
    {

        foreach (var binding in bindings)
        {
            KeyCode key = binding.key;

            if (Input.GetKeyDown(key))
            {
                _pressDurations[key] = 0f;

                var info = GetInfo(EInputType.Down,binding.actionName,0);
                for (int i = 0; i < _inputSubscribers.Count; i++)
                {
                    _inputSubscribers[i].OnInput(info);
                }
            }
            if (Input.GetKey(key))
            {
                if (_pressDurations.ContainsKey(key))
                {
                    _pressDurations[key] += Time.deltaTime;

                    var info = GetInfo(EInputType.Press, binding.actionName,_pressDurations[key]);
                    for (int i = 0; i < _inputSubscribers.Count; i++)
                    {
                        _inputSubscribers[i].OnInput(info);
                    }
                }
            }
            if (Input.GetKeyUp(key))
            {
                var info = GetInfo(EInputType.Up, binding.actionName,0);
                for (int i = 0; i < _inputSubscribers.Count; i++)
                {
                    _inputSubscribers[i].OnInput(info);
                }
                _pressDurations.Remove(key); // 끝났으니 타이머 제거
            }
        }
    }


    private InputInfo GetInfo(EInputType type ,string actionName,float duration = 0)
    {
        var info = new InputInfo
        {
            inputType = type,
            deltaTime = duration,
            actionName = actionName
        };
        return info;
    }
    public static void RegisterInput(IInputSubscriber inputSubscriber)
    { 
        _inputSubscribers.Add(inputSubscriber);
    }
    public static void UnRegisterInput(IInputSubscriber inputSubscriber)
    {
        _inputSubscribers.Remove(inputSubscriber);
    }
}