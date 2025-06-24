using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLighting : MonoBehaviour
{
    public Material skyBoxMaterial;
    void Awake()
    {
        RenderSettings.skybox = skyBoxMaterial;
        DynamicGI.UpdateEnvironment();

    }

}
