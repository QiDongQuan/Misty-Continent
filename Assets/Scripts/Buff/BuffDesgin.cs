using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffUpdateTimeEnum
{
    Add,//叠层时叠加时间
    Replace,//叠层时刷新时间
    Keep//叠层时时间不变
}

public enum BuffRemoveStackUpdataEnum
{
    Clear,//时间结束buff直接消失
    Reduce//时间结束减少层数
}

[System.Serializable]
public class BuffInfo
{
    public BuffData buffData;
    public GameObject creator;//创建者
    public GameObject target;//目标
    public float durationTimer;//持续时间
    public float tickTimer;//持续期间生效间隔
    public int curStack = 1;//当前层数
    public string tmp;
    public float[] effect;

    public BuffInfo(BuffData buffData, GameObject creator, GameObject target)
    {
        this.buffData = buffData;
        this.creator = creator;
        this.target = target;
    }
}

public class DamageInfo
{
    public GameObject creator;
    public GameObject target;
    public int damage;

    public DamageInfo(GameObject creator, GameObject target, int damage)
    {
        this.creator = creator;
        this.target = target;
        this.damage = damage;
    }
}
