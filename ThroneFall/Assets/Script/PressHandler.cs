using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressHandler : MonoBehaviour
{
    // private Vector3 clickPos;
    // public bool isPressing = false;
    // private float pressStartTime = 0f;
    // private const float holdDuration = 2f;
    //
    // public LayerMask SelectLayer;
    //
    // public bool isPress;
    //
    // private Town SelectTown;
    // private void Update()
    // {
    //     
    //     if (Input.GetMouseButtonDown(0)) // 클릭 시작
    //     {
    //         if (EventSystem.current.IsPointerOverGameObject())
    //         {
    //             return;
    //         }
    //         clickPos = Input.mousePosition;
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         if (Physics.Raycast(ray, out RaycastHit hit))
    //         {
    //             isPressing = true;
    //             pressStartTime = Time.time;
    //             SelectLayer = hit.collider.gameObject.layer;
    //                         
    //             // Town이 우선
    //             if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Town")||hit.collider.gameObject.layer == LayerMask.NameToLayer("PreTown"))
    //             {
    //                 SelectTown = hit.collider.GetComponent<Town>();
    //             }
    //
    //             else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //             {
    //                 // Ground 처리
    //             }
    //                         
    //         } 
    //     }
    //     if (isPressing) // 지속 클릭 중일 때
    //     {
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         if (Physics.Raycast(ray, out RaycastHit hit))
    //         {
    //         }
    //         else // Raycast가 아무것도 감지하지 못하면 취소
    //         {
    //             CancelPress();
    //             return;
    //         }
    //
    //         if (Input.GetMouseButton(0))
    //         {
    //             isPressing = OnMousePressHold(Time.time - pressStartTime, holdDuration);
    //             //isPressing = false; // 이벤트 발생 후 해제
    //         }
    //     }
    //
    //     if (Input.GetMouseButtonUp(0) && isPressing) // 클릭 해제
    //     {
    //         if (isPressing) // 클릭 지속 시간이 holdDuration 미만이면 클릭 이벤트 실행
    //         {
    //             Click();
    //         }
    //         CancelPress();
    //         isPressing = false;
    //     } 
    //         
    //     
    // }
    //
    // public bool OnMousePressHold(float duration, float maxValue)
    // {
    //     if (SelectLayer == LayerMask.NameToLayer("Ground"))
    //     {
    //         return InGame.GameRound.OnMousePressHold(duration, maxValue, clickPos);
    //     }
    //     else if (SelectLayer == LayerMask.NameToLayer("Town")||SelectLayer == LayerMask.NameToLayer("PreTown"))
    //     {
    //         return InGame.townManager.OnMousePressHold(duration, maxValue, SelectTown, clickPos);
    //     }
    //
    //     return true;
    // }
    //
    // private void Click()
    // {
    //     if (SelectLayer == LayerMask.NameToLayer("Ground"))
    //     {
    //         
    //     }
    //     else if (SelectLayer == LayerMask.NameToLayer("Town")||SelectLayer == LayerMask.NameToLayer("PreTown"))
    //     {
    //         SelectTown.OnClickTown();
    //     }
    // }
    //
    // private void CancelPress()
    // {
    //     isPressing = false;
    //     SelectTown = null;
    //     InGame.MousePressProgress.SetActive(false);
    //     AudioController.instance.Stop(SoundConfig.SoundType.Effect2);
    //
    //     if (SelectLayer == LayerMask.NameToLayer("Ground"))
    //     {
    //         InGame.GameRound.CancelPress();
    //     }
    //     else if (SelectLayer == LayerMask.NameToLayer("Town"))
    //     {
    //         InGame.townManager.CancelPress();
    //     }
    // }
    //
    // private InGameManager InGame => InGameManager.Call;
}