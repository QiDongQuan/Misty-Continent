using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float runSpeed = 5.0f;

    public int Hp;//Ѫ��
    public int Enenrgy;//����
    public int Attack;//����
    public int Armor;//����

    public bool IsCanBeKill(DamageInfo damageInfo)
    {
        if(Hp <= damageInfo.damage)
        {
            return true;
        }
        return false;
    }

    public virtual void GetHit(int damage)
    {

    }
}
