using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPressProgressProvider
{
    float CurrentPressProgress { get; }
    void SetPressProgress(float time);
    void StartPress();
}
