using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputSettings
{
    public List<InputBinding> _inputBindings = new();
}

[System.Serializable]
public class InputBinding
{
    public string actionName;    
    public KeyCode key;          
}
public interface IInputSubscriber
{
    void OnInput(InputInfo info);
}