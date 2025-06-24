using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public Transform listenerTf;
    public Vector3 posOffset = new Vector3(0, 5, -10);  // 카메라 위치 오프셋
    public Vector3 lookOffset = new Vector3(30, 0, 0);   // Euler 각도 (X=아래보기)

    public List<IUpdate> updateList = new List<IUpdate>();
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (followTarget != null)
        {
            // 대상 기준 위치 이동
            Vector3 targetPos = followTarget.position + posOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

            // 회전: lookOffset을 Euler 각도로 사용
            transform.rotation = Quaternion.Euler(lookOffset);
        }
    }
}