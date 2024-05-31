using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemJsonData
{
    public int ID { get; }
    public string Name { get; }
    public int Type { get; }
    public int[] Attack { get; }
    public int[] Defensive { get; }
    public int[] Hp { get; }
    public string PrefabPath { get; }
    public int BuffId { get; }
    public string IamgePath { get; }

    public ItemJsonData(int ID, string Name, int Type, int[] Attack, int[] Defensive, int[] Hp, string PrefabPath, int BuffId, string iamgePath)
    {
        this.ID = ID;
        this.Name = Name;
        this.Type = Type;
        this.Attack = Attack;
        this.Defensive = Defensive;
        this.Hp = Hp;
        this.PrefabPath = PrefabPath;
        this.BuffId = BuffId;
        this.IamgePath = iamgePath;
    }
}

public class JsonManager
{
    public Dictionary<int, ItemJsonData> items;

    static JsonManager instance;
    public static JsonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new JsonManager();
                instance.LodingItemData();
            }
            return instance;
        }
    }

    private void LodingItemData()
    {
        items = new Dictionary<int, ItemJsonData>();
        string str = Resources.Load<TextAsset>("JsonData/ItemData").text;
        JsonData data = JsonMapper.ToObject(str);
        for (int i = 0; i < data.Count; i++)
        {
            int id = int.Parse(data[i]["ID"].ToString());
            items.Add(id, new ItemJsonData(id,
                data[i]["Name"].ToString(),
                int.Parse(data[i]["Type"].ToString()),
                GetEffect(data[i]["Attack"].ToString()),
                GetEffect(data[i]["Defensive"].ToString()),
                GetEffect(data[i]["Hp"].ToString()),
                data[i]["PrefabPath"].ToString(),
                int.Parse(data[i]["BuffId"].ToString()),
                data[i]["ImagePath"].ToString()
                ));
        }
    }

    private int[] GetEffect(string effects)
    {
        string[] jsonEffect = effects.Split('-');
        return jsonEffect.Select(int.Parse).ToArray();
    }

    public ItemJsonData GetJson(int id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        Debug.LogError($"id为{id}的道具不存在");
        return null;
    }
}