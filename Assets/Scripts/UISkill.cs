using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkill : MonoBehaviour
{
    [HideInInspector]
    public BuffInfo buffInfo;
    Transform cd;

    private void Start()
    {
        cd = transform.GetChild(0);
    }

    private void Update()
    {
        cd.GetComponent<Image>().fillAmount = buffInfo.tickTimer/buffInfo.buffData.tickTime;
    }
}
