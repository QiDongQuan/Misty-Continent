using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffJsonData
{
    public int ID { get; }
    public string Path { get; }

    public BuffJsonData(int ID, string Path)
    {
        this.ID = ID;
        this.Path = Path;
    }
}

public class BuffJsonManager
{
    public Dictionary<int, BuffJsonData> items;

    static BuffJsonManager instance;
    public static BuffJsonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuffJsonManager();
                instance.LodingItemData();
            }
            return instance;
        }
    }

    private void LodingItemData()
    {
        items = new Dictionary<int, BuffJsonData>();
        string str = Resources.Load<TextAsset>("JsonData/BuffData").text;
        JsonData data = JsonMapper.ToObject(str);
        for (int i = 0; i < data.Count; i++)
        {
            int id = int.Parse(data[i]["ID"].ToString());
            items.Add(id, new BuffJsonData(id,
                data[i]["Path"].ToString()
                ));
        }
    }

    public BuffJsonData GetJson(int id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        Debug.LogError($"id为{id}的怪物不存在");
        return null;
    }
}
