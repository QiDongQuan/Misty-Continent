using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapJsonData
{
    public int ID { get; }
    public int Attack { get; }
    public int Armor { get; }
    public int Hp { get; }
    public int Count { get; }
    public int[] DropID { get; }

    public MapJsonData(int id, int attack, int armor, int hp, int count, int[] dropId)
    {
        this.ID = id;
        Attack = attack;
        Armor = armor;
        Hp = hp;
        Count = count;
        DropID = dropId;
    }
}

public class MapEnemiesData
{
    public List<MapJsonData> Enemies { get; set; }
    public List<MapJsonData> Bosses { get; set; }

    public MapEnemiesData()
    {
        Enemies = new List<MapJsonData>();
        Bosses = new List<MapJsonData>();
    }
}

public class MapJsonManager
{
    public Dictionary<int, MapEnemiesData> enemeies;

    static MapJsonManager instance;
    public static MapJsonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MapJsonManager();
                instance.LodingItemData();
            }
            return instance;
        }
    }

    private void LodingItemData()
    {
        enemeies = new Dictionary<int, MapEnemiesData>();
        string str = Resources.Load<TextAsset>("JsonData/MapEnemyData").text;
        JsonData data = JsonMapper.ToObject(str);

        // 遍历JSON中的每一个关键词（例如 "1001"）
        foreach (string key in data.Keys)
        {
            int mapId = int.Parse(key);
            MapEnemiesData mapData = new MapEnemiesData();

            // 获取"Enemies"数组
            JsonData enemiesData = data[key]["Enemies"];
            for (int i = 0; i < enemiesData.Count; i++)
            {
                MapJsonData enemy = new MapJsonData(
                    id: int.Parse(enemiesData[i]["ID"].ToString()),
                    attack: int.Parse(enemiesData[i]["Attack"].ToString()),
                    armor: int.Parse(enemiesData[i]["Armor"].ToString()),
                    hp: int.Parse(enemiesData[i]["Hp"].ToString()),
                    count: int.Parse(enemiesData[i]["Count"].ToString()),
                    dropId: GetEffect(enemiesData[i]["DropID"].ToString())
                // 追加其他需要的参数
                );
                mapData.Enemies.Add(enemy);
            }

            // “Boss”假设只有一个Boss
            JsonData bossData = data[key]["Boss"];
            for (int i = 0; i < bossData.Count; i++)
            {
                MapJsonData boss = new MapJsonData(
                id: int.Parse(bossData[i]["ID"].ToString()),
                attack: int.Parse(bossData[i]["Attack"].ToString()),
                armor: int.Parse(bossData[i]["Armor"].ToString()),
                hp: int.Parse(bossData[i]["Hp"].ToString()),
                count: int.Parse(bossData[i]["Count"].ToString()),
                dropId: GetEffect(bossData[i]["DropID"].ToString())
            // 追加其他需要的参数
                );
                mapData.Bosses.Add(boss);
            }
            enemeies.Add(mapId, mapData);
        }
    }

    public MapEnemiesData GetJson(int id)
    {
        if (enemeies.ContainsKey(id))
        {
            return enemeies[id];
        }
        Debug.LogError($"id为{id}的关卡不存在");
        return null;
    }

    private int[] GetEffect(string effects)
    {
        string[] jsonEffect = effects.Split('-');
        return jsonEffect.Select(int.Parse).ToArray();
    }
}