using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class SectorBullet : MonoBehaviour
{
    public GameObject creator;
    public GameObject bulletPrefab; // �ӵ���Ԥ����
    public float aliveTime = 10;
    public float speed = 16;
    public int bulletCount = 5; // �����ӵ�������
    public float spreadAngle = 15f; // �ӵ�֮�������ɢ�Ƕ�

    private void Start()
    {
        creator = GameObject.FindGameObjectWithTag("Player");
        GenerateFanBullets();
    }

    void GenerateFanBullets()
    {
        Vector3 firePointPosition = creator.GetComponent<PlayerCharacter>().firePoint.position;
        Vector3 targetPosition = creator.GetComponent<PlayerCharacter>().target.position;

        // ���㳯��Ŀ��ķ�������
        Vector3 directionToTarget = (targetPosition - firePointPosition).normalized;

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePointPosition, Quaternion.identity);
            NormalBullet bulletScript = bullet?.GetComponent<NormalBullet>();
            if (bulletScript == null)
            {
                bulletScript = bullet.AddComponent<NormalBullet>();
            }

            // �����ӵ���ʼ����
            bulletScript.creator = creator;
            bulletScript.aliveTime = aliveTime;
            bulletScript.speed = speed;

            // ����ÿ���ӵ���ƫת�Ƕ�
            float angle = (spreadAngle / (bulletCount - 1)) * i - (spreadAngle / 2);
            // �������ճ���Ƕȣ���Ӧ�õ��ӵ���
            Vector3 finalDirection = Quaternion.Euler(0, 0, angle) * directionToTarget;
            bullet.transform.right = finalDirection;
        }
        Destroy(gameObject);
    }
}
