using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // �����Ŀ��

    void FixedUpdate()
    {
        Vector3 targetPosWithOffset = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = targetPosWithOffset;
    }
}
