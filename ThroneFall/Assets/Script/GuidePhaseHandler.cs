using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GuidePhaseHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> Arrows = new List<GameObject>();
     [SerializeField] private GuidePanel guidePanel;
    private int _currentPhase;

    private void Start()
    {
        Arrows[0].SetActive(true);
        guidePanel.SetPanel(0);
        _currentPhase = 0;
    }

    public void NextPhase()
    {
        _currentPhase++;

        NextArrow();
        guidePanel.SetPanel(_currentPhase);

    }

    public void NextArrow()
    {
        if (_currentPhase <= Arrows.Count-1)
        {
            Arrows[_currentPhase].SetActive(true);
            Arrows[_currentPhase-1].SetActive(false);
        }
        else
        {
            foreach (var arrow in Arrows)
            {
                if (arrow != null)
                {
                    arrow.SetActive(false);
                }
            }
        }

    }

    public void NextPanel()
    {

    }

}
