using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameResultProvider
{
    void NotifyGameOver();
    void NotifyGameClear();
}
