using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerInput : MonoBehaviour
{
    private Move Mover;
    private void Start()
    {
        Mover = GetComponent<Move>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Mover._isWantMove = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point; // 클릭한 지점
                Mover.dest = targetPosition;
                Mover.OnChangeDest();
            }
        }

    }
    
}