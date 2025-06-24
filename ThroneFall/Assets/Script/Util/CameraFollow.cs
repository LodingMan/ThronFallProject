using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public Transform listenerTf;
    public Vector3 posOffset = new Vector3(0, 5, -10);  // ī�޶� ��ġ ������
    public Vector3 lookOffset = new Vector3(30, 0, 0);   // Euler ���� (X=�Ʒ�����)

    public List<IUpdate> updateList = new List<IUpdate>();
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (followTarget != null)
        {
            // ��� ���� ��ġ �̵�
            Vector3 targetPos = followTarget.position + posOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

            // ȸ��: lookOffset�� Euler ������ ���
            transform.rotation = Quaternion.Euler(lookOffset);
        }
    }
}