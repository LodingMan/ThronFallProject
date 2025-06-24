using System.Collections;
using UnityEngine;

public class FollowSpawnerUI : MonoBehaviour
{
    public int Index;
    public Transform targetSpawner;
    public RectTransform myUI;
    public Canvas canvas;
    public Camera mainCam;
    public float screenBorder = 30f;
    public float yOffset = 50f;
    public float smoothSpeed = 500f;

    private Vector3 smoothedPosition;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        mainCam = Camera.main;
        if (myUI != null && targetSpawner != null)
        {
            // 초기 위치 설정
            UpdateUIPosition(true);
        }
    }

    private void LateUpdate()
    {
        if (targetSpawner == null || canvas == null || mainCam == null || myUI == null) return;
        
        UpdateUIPosition(false);
    }

    private void UpdateUIPosition(bool immediate)
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(targetSpawner.position);
        screenPos.y += yOffset;

        // 화면 밖 여부 판단
        bool isBehind = screenPos.z < 0;
        
        // z < 0이면 반대 방향에서 들어온 것이므로 좌표 반전
        if (isBehind)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        // Clamp 처리 + 픽셀 스냅
        screenPos.x = Mathf.Round(Mathf.Clamp(screenPos.x, screenBorder, Screen.width - screenBorder));
        screenPos.y = Mathf.Round(Mathf.Clamp(screenPos.y, screenBorder, Screen.height - screenBorder));

        // 화면 좌표 → UI 월드 좌표로 변환
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.worldCamera,
                out Vector3 targetWorldPos))
        {
            if (immediate)
            {
                smoothedPosition = targetWorldPos;
                myUI.position = targetWorldPos;
            }
            else
            {
                // 부드러운 이동을 위해 SmoothDamp 사용
                smoothedPosition = Vector3.SmoothDamp(
                    myUI.position, 
                    targetWorldPos, 
                    ref velocity, 
                    smoothSpeed * Time.deltaTime);
                
                myUI.position = smoothedPosition;
            }
        }
    }
}