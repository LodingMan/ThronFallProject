using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MousePressProgress : MonoBehaviour
{
    public Image imgProgress;
    public TMP_Text lbProgress;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void SetProgress(float progress)
    {
        if (progress == 0)
        {
            SetActive(false);
            //transform.position = Input.mousePosition;
            return;
        }
        //Debug.Log($"Progress : {progress}");
        SetActive(true);
        //imgProgress.fillAmount = progress;
        int percent = (int)(progress * 100);
        lbProgress.text = $"{percent}%";
        if (percent == 100)
        {
            SetActive(false);
        }
    }
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

}
