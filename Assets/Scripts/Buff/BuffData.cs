using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_BuffData", menuName = "BuffSystem/BuffData", order = 1)]
public class BuffData : ScriptableObject
{//������Ϣ
    public int id;
    public string buffName;
    public string desc;//����
    public Sprite icon;
    public int priority;//���ȼ�
    public int maxStack;//������
    public string[] tags;//��¼buff����

    //ʱ����Ϣ
    public bool isForever;//�Ƿ�����
    public float duration;//����ʱ��
    public float tickTime;//������Ч���ʱ��

    //���·�ʽ
    public BuffUpdateTimeEnum buffUpdateTime;
    public BuffRemoveStackUpdataEnum buffRemoveStackUpdata;

    //�����ص���
    public BaseBuffModule OnCreate;
    public BaseBuffModule OnRemove;
    public BaseBuffModule OnTick;
    //�˺��ص���
    public BaseBuffModule OnHit;
    public BaseBuffModule OnBehurt;
    public BaseBuffModule OnKill;
    public BaseBuffModule OnBeKill;
}
