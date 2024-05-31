using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int autoID;
    public int jsonID;
    public int quality;
    public int lv;

    public ItemJsonData jsonData
    {
        get { return JsonManager.Instance.GetJson(jsonID); }
    }
}