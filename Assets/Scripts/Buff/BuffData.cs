using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_BuffData", menuName = "BuffSystem/BuffData", order = 1)]
public class BuffData : ScriptableObject
{//基本信息
    public int id;
    public string buffName;
    public string desc;//描述
    public Sprite icon;
    public int priority;//优先级
    public int maxStack;//最大层数
    public string[] tags;//记录buff类型

    //时间信息
    public bool isForever;//是否永久
    public float duration;//持续时间
    public float tickTime;//持续生效间隔时间

    //更新方式
    public BuffUpdateTimeEnum buffUpdateTime;
    public BuffRemoveStackUpdataEnum buffRemoveStackUpdata;

    //基础回调点
    public BaseBuffModule OnCreate;
    public BaseBuffModule OnRemove;
    public BaseBuffModule OnTick;
    //伤害回调点
    public BaseBuffModule OnHit;
    public BaseBuffModule OnBehurt;
    public BaseBuffModule OnKill;
    public BaseBuffModule OnBeKill;
}
