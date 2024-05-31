using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class SectorBullet : MonoBehaviour
{
    public GameObject creator;
    public GameObject bulletPrefab; // 子弹的预制体
    public float aliveTime = 10;
    public float speed = 16;
    public int bulletCount = 5; // 发射子弹的数量
    public float spreadAngle = 15f; // 子弹之间的总扩散角度

    private void Start()
    {
        creator = GameObject.FindGameObjectWithTag("Player");
        GenerateFanBullets();
    }

    void GenerateFanBullets()
    {
        Vector3 firePointPosition = creator.GetComponent<PlayerCharacter>().firePoint.position;
        Vector3 targetPosition = creator.GetComponent<PlayerCharacter>().target.position;

        // 计算朝向目标的方向向量
        Vector3 directionToTarget = (targetPosition - firePointPosition).normalized;

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePointPosition, Quaternion.identity);
            NormalBullet bulletScript = bullet?.GetComponent<NormalBullet>();
            if (bulletScript == null)
            {
                bulletScript = bullet.AddComponent<NormalBullet>();
            }

            // 设置子弹初始属性
            bulletScript.creator = creator;
            bulletScript.aliveTime = aliveTime;
            bulletScript.speed = speed;

            // 计算每个子弹的偏转角度
            float angle = (spreadAngle / (bulletCount - 1)) * i - (spreadAngle / 2);
            // 计算最终朝向角度，并应用到子弹上
            Vector3 finalDirection = Quaternion.Euler(0, 0, angle) * directionToTarget;
            bullet.transform.right = finalDirection;
        }
        Destroy(gameObject);
    }
}
