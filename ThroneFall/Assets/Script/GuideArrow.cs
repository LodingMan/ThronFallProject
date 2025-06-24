using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrow : MonoBehaviour
{
    private GuidePhaseHandler _handler;
    private void Awake()
    {
        _handler = FindObjectOfType<GuidePhaseHandler>();
        this.gameObject.SetActive(false);
    }

    public void RequestNextPhase()
    {
        _handler.NextPhase();
    }
}
