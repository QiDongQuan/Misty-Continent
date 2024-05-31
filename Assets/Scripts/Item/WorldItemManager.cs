using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemManager : MonoBehaviour
{
    public Dictionary<int, ItemData> allItems = new Dictionary<int, ItemData>();
    public static WorldItemManager Instance;
    [HideInInspector]
    public int count = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //���Ʒ��1-3���ɵ���
    public ItemData CreateItem(int jsonId)
    {
        ItemData itemData = new ItemData();
        itemData.autoID = count;
        itemData.jsonID = jsonId;
        if (itemData.jsonData == null)
        {
            return null;
        }
        itemData.quality = Random.Range(1, 4);
        itemData.lv = 1;
        allItems.Add(count, itemData);
        count++;
        return itemData;
    }

    //ָ��Ʒ�����ɵ���
    public ItemData CreateItem(int jsonId, int quality)
    {
        if (quality < 1 || quality > 3)
        {
            Debug.Log("����Ʒ��Ӧָ����1-3��Χ��");
            return null;
        }
        ItemData itemData = new ItemData();
        itemData.autoID = count;
        itemData.jsonID = jsonId;
        if (itemData.jsonData == null)
        {
            return null;
        }
        itemData.quality = quality;
        itemData.lv = 1;
        allItems.Add(count, itemData);
        count++;
        return itemData;
    }

    public void RemoveItem(int autoID)
    {
        allItems.Remove(autoID);
    }
}
