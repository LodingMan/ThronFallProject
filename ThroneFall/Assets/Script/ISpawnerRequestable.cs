using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnerRequestable
{
    void RequestInitializeSpawner(ISpawnerInitializer initializer);


}
