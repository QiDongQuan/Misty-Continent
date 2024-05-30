using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void SubmitDamage(DamageInfo damageInfo)
    {
        BuffHandler creatorBuffHandler = damageInfo.creator?.GetComponent<BuffHandler>();
        BuffHandler targetBuffHandler = damageInfo.target?.GetComponent<BuffHandler>();

        if (creatorBuffHandler)
        {
            foreach (var buffInfo in creatorBuffHandler.buffList)
            {
                buffInfo.buffData.OnHit?.Applay(buffInfo, damageInfo);
            }
        }

        if (targetBuffHandler)
        {
            foreach (var buffInfo in targetBuffHandler.buffList)
            {
                buffInfo.buffData.OnBehurt?.Applay(buffInfo, damageInfo);
            }

            //var component = damageInfo.target.GetComponent<Character>();
            Character component = null;
            if (damageInfo.target.CompareTag("Player"))
            {
                component = damageInfo.target.GetComponent<PlayerCharacter>();
            }
            else if (damageInfo.target.CompareTag("Enemy"))
            {
                component = damageInfo.target.GetComponent<EnemyCharacter>();
            }
            else if (damageInfo.target.CompareTag("Boss"))
            {
                component = damageInfo.target.GetComponent<BossCharacter>();
            }

            if (component)
            {
                if (component.IsCanBeKill(damageInfo))
                {
                    foreach (var buffInfo in targetBuffHandler.buffList)
                    {
                        buffInfo.buffData.OnBeKill?.Applay(buffInfo, damageInfo);
                    }

                    if (component.IsCanBeKill(damageInfo))
                    {
                        if (creatorBuffHandler)
                        {
                            foreach (var buffInfo in creatorBuffHandler.buffList)
                            {
                                buffInfo.buffData.OnKill?.Applay(buffInfo, damageInfo);
                            }
                        }
                    }
                }
                if (damageInfo.damage > 0)
                {
                    component.GetHit(damageInfo.damage);
                }
            }
        }
    }
}
