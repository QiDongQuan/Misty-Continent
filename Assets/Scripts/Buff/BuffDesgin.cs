using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffUpdateTimeEnum
{
    Add,//����ʱ����ʱ��
    Replace,//����ʱˢ��ʱ��
    Keep//����ʱʱ�䲻��
}

public enum BuffRemoveStackUpdataEnum
{
    Clear,//ʱ�����buffֱ����ʧ
    Reduce//ʱ��������ٲ���
}

[System.Serializable]
public class BuffInfo
{
    public BuffData buffData;
    public GameObject creator;//������
    public GameObject target;//Ŀ��
    public float durationTimer;//����ʱ��
    public float tickTimer;//�����ڼ���Ч���
    public int curStack = 1;//��ǰ����
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
