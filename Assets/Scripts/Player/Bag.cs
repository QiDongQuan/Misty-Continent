using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class Bag : MonoBehaviour
{
    [HideInInspector]
    public List<ItemData> items = new List<ItemData>();//玩家背包数据
    [HideInInspector]
    public List<ItemData> playerItems = new List<ItemData>();//玩家装备槽
    public const int n = 20;//玩家背包容量
    PlayerCharacter player;

    private void Start()
    {
        player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("添加道具");
            AddRandomItem();
        }
    }

    public int FindFirstEmpty()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                return i;
            }
        }
        if (items.Count == n)
        {
            UIManager.Instance.SetTips("背包已满", 2);
            Debug.Log("背包已满");
            return -1;
        }
        items.Add(null);
        return items.Count - 1;
    }

    public void Refresh()
    {
        for(int i = 0;i < items.Count;i++)
        {
            if (items[i].autoID == 0)
            {
                continue;
            }
            UIManager.Instance.SetItem(i, items[i]);
        }
        for(int i = 0;i<playerItems.Count;i++)
        {
            if (playerItems[i].autoID == 0)
            {
                continue;
            }
            UIManager.Instance.SetItem(i, playerItems[i], UIManager.Instance.playerGrid);
        }
    }

    //随机添加道具
    public void AddRandomItem()
    {
        ItemData item = WorldItemManager.Instance.CreateItem(Random.Range(1001, 1004));
        int index = FindFirstEmpty();
        if (index == -1)
        {
            return;
        }
        items[index] = item;
        UIManager.Instance.SetItem(index, item);
    }

    public void AddItem(ItemData item)
    {
        int index = FindFirstEmpty();
        if (index == -1)
        {
            return;
        }
        items[index] = item;
        UIManager.Instance.SetItem(index, item);
    }

    //删除背包指定道具
    public void RemoveItem(ItemData item, Transform from)
    {
        int index = -1;
        if (from == UIManager.Instance.grid)
        {
            index = items.IndexOf(item);
        }
        else if (from == UIManager.Instance.playerGrid)
        {
            index = playerItems.IndexOf(item);
        }
        if (index == -1)
        {
            Debug.Log($"删除的道具{item.jsonData.Name}不存在");
            return;
        }
        if (from == UIManager.Instance.grid)
        {
            items[index] = null;
        }
        else if (from == UIManager.Instance.playerGrid)
        {
            playerItems[index] = null;
        }
        WorldItemManager.Instance.RemoveItem(item.autoID);
        UIManager.Instance.SetItem(index, null, from);
    }

    //交换背包道具位置
    public void SwapItem(ItemData item, int index, List<ItemData> list, Transform grid)
    {
        string from = "items";
        int n = items.IndexOf(item);
        if (n == -1)
        {
            n = playerItems.IndexOf(item);
            from = "playerItems";
        }
        if (n == -1)
        {
            Debug.Log($"道具{item.jsonData.Name}不存在");
        }

        for (int i = list.Count; i <= index; i++)
        {
            list.Add(null);
        }

        if (from == "items")
        {
            items[n] = list[index];
            UIManager.Instance.SetItem(n, items[n]);
        }
        else
        {
            playerItems[n] = list[index];
            UIManager.Instance.SetItem(n, playerItems[n], UIManager.Instance.playerGrid);
        }

        list[index] = item;
        UIManager.Instance.SetItem(index, item, grid);
    }

    //道具被拖走但无操作需要复原
    public void RestoreItem(ItemData item, Transform from)
    {
        int index = -1;
        if (from == UIManager.Instance.grid)
        {
            index = items.IndexOf(item);
        }
        else if (from == UIManager.Instance.playerGrid)
        {
            index = playerItems.IndexOf(item);
        }

        if (index == -1)
        {
            Debug.Log($"道具{item.jsonData.Name}不存在");
        }

        UIManager.Instance.SetItem(index, item, from);
    }
}
