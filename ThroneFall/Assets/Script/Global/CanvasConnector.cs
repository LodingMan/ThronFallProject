using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasConnector : MonoBehaviour
{
    Canvas CameraCanvas;
    void Start()
    {
        CameraCanvas = GetComponent<Canvas>();
        CameraCanvas.worldCamera = Camera.main;
        CameraCanvas.planeDistance = 5;  
    }


}
