using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyJsonData
{
    public int ID { get; }
    public string PrefabPath { get; }

    public EnemyJsonData(int ID, string Prefab)
    {
        this.ID = ID;
        this.PrefabPath = Prefab;
    }
}

public class EnemyJsonManager
{
    public Dictionary<int, EnemyJsonData> items;

    static EnemyJsonManager instance;
    public static EnemyJsonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyJsonManager();
                instance.LodingItemData();
            }
            return instance;
        }
    }

    private void LodingItemData()
    {
        items = new Dictionary<int, EnemyJsonData>();
        string str = Resources.Load<TextAsset>("JsonData/EnemyData").text;
        JsonData data = JsonMapper.ToObject(str);
        for (int i = 0; i < data.Count; i++)
        {
            int id = int.Parse(data[i]["ID"].ToString());
            items.Add(id, new EnemyJsonData(id,
                data[i]["PrefabPath"].ToString()
                ));
        }
    }

    public EnemyJsonData GetJson(int id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        Debug.LogError($"id为{id}的怪物不存在");
        return null;
    }
}
