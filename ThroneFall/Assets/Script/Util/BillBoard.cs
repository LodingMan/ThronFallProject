using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // 단 하나의 카메라 참조

    }


    // Update is called once per frame
    void Update()
    {
        // if (Camera.main != null)
        // {
        //     Vector3 cameraForward = mainCamera.transform.forward;
        //     transform.forward = new Vector3(cameraForward.x, transform.forward.y, cameraForward.z);
        // }

    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}
