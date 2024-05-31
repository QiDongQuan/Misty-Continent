using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBuff : MonoBehaviour
{
    [HideInInspector]
    public List<BuffInfo> buffDatas = new List<BuffInfo>();
    public Transform skillUI;
    public Image skillIcon;

    public void Refresh()
    {
        for (int i = 0; i<5 && i < buffDatas.Count; i++)
        {
            SetSkill(i, buffDatas[i]);
        }
    }

    public void SetSkill(int index,BuffInfo buffInfo)
    {
        Transform solt = skillUI.GetChild(index);
        if(solt.childCount > 0)
        {
            Destroy(solt.GetChild(0).gameObject);
        }

        if(buffInfo != null)
        {
            Image image = Instantiate(skillIcon, solt);
            image.GetComponent<UISkill>().buffInfo = buffInfo;
            image.sprite = buffInfo.buffData?.icon;
        }
    }
}
