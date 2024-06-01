using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float runSpeed = 5.0f;

    public int Hp;//血量
    public int Enenrgy;//能量
    public int Attack;//攻击
    public int Armor;//护甲

    public int MaxHp;
    public int MaxEnenrgy;

    public int DropExperience;//怪物被击杀时玩家获得的经验
    public int DropEnergy;//怪物被击杀时玩家获得的能量

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
