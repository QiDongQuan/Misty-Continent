using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class Bag : MonoBehaviour
{
    [HideInInspector]
    public List<ItemData> items = new List<ItemData>();//��ұ�������
    [HideInInspector]
    public List<ItemData> playerItems = new List<ItemData>();//���װ����
    public const int n = 20;//��ұ�������
    PlayerCharacter player;

    private void Start()
    {
        player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("��ӵ���");
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
            UIManager.Instance.SetTips("��������", 2);
            Debug.Log("��������");
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

    //�����ӵ���
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

    //ɾ������ָ������
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
            Debug.Log($"ɾ���ĵ���{item.jsonData.Name}������");
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

    //������������λ��
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
            Debug.Log($"����{item.jsonData.Name}������");
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

    //���߱����ߵ��޲�����Ҫ��ԭ
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
            Debug.Log($"����{item.jsonData.Name}������");
        }

        UIManager.Instance.SetItem(index, item, from);
    }
}
