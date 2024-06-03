using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class GameMode : MonoBehaviour
{
    public static GameMode Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    [System.Serializable]
    class SaveData
    {
        public int Lv;
        public int experience;
        public List<ItemData> items;
        public List<ItemData> playerItems;
        public List<string> buffList;
        public List<allItemsKeyValue> allItems;
        public int itemsAutoID;
    }

    [System.Serializable]
    class allItemsKeyValue
    {
        public int Key;
        public ItemData Value;
        public allItemsKeyValue(int key, ItemData value)
        {
            Key = key;
            Value = value;
        }
    }

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        LoadGame();
    }

    public string CreateGameLoad()
    {
        string path = Path.Combine(Application.persistentDataPath, "GameData");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(CreateGameLoad(), saveFileName);

        try
        {
            File.WriteAllText(path, json);

#if UNITY_EDITOR
            Debug.Log($"存档成功保存到{path}");
#endif
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"存档保存到{path}.\n{exception}");
#endif
        }
    }

    public T LoadByJson<T>(string saveFileName)
    {
        var path = Path.Combine(CreateGameLoad(), saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"存档从{path}读取.\n{exception}");
#endif
            return default;
        }
    }

    SaveData SavingData()
    {
        var saveData = new SaveData();

        saveData.Lv = player.GetComponent<PlayerCharacter>().lv;
        saveData.experience = player.GetComponent<PlayerCharacter>().experience;
        saveData.items = new List<ItemData>(player.GetComponent<Bag>().items);
        saveData.playerItems = new List<ItemData>(player.GetComponent<Bag>().playerItems);
        saveData.buffList = new List<string>();
        foreach (var buff in player.GetComponent<BuffHandler>().buffList)
        {
            if (buff.buffData.isSkill)
            {
                saveData.buffList.Add(BuffJsonManager.Instance.GetJson(buff.buffData.id).Path);
            }
        }
        saveData.allItems = new List<allItemsKeyValue>();
        foreach (var item in WorldItemManager.Instance.allItems)
        {
            saveData.allItems.Add(new allItemsKeyValue(item.Key, item.Value));
        }
        saveData.itemsAutoID = WorldItemManager.Instance.count;

        return saveData;
    }

    void LoadData(SaveData saveData)
    {
        player.GetComponent<PlayerCharacter>().lv = saveData.Lv;
        player.GetComponent<PlayerCharacter>().experience = saveData.experience;
        for (int i=0;i< saveData.items.Count;i++)
        {
            player.GetComponent<Bag>().items.Add(saveData.items[i]);
        }
        player.GetComponent<Bag>().playerItems = new List<ItemData>(saveData.playerItems);
        foreach (var buffpath in saveData.buffList)
        {
            Debug.Log(buffpath);
            if (!player)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            if (player != null)
            {
                Debug.Log($"PlayerCharacter 组件是否存在: {player.GetComponent<PlayerCharacter>() != null}");
            }
            try
            {
                player.GetComponent<PlayerCharacter>().AddBuff(buffpath, player, player);
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("发生了 NullReferenceException: " + ex.Message);
            }
            
        }
        player.GetComponent<Bag>().Refresh();
        WorldItemManager.Instance.allItems.Clear();
        foreach (var item in saveData.allItems)
        {
            WorldItemManager.Instance.allItems.TryAdd(item.Key, item.Value);
        }
        WorldItemManager.Instance.count = saveData.itemsAutoID;
    }

    public void SaveGame()
    {
        SaveByJson("GameData", SavingData());
    }

    public void LoadGame()
    {
        var path = Path.Combine(CreateGameLoad(), "GameData");

        if (!File.Exists(path))
        {
            return;
        }

        var saveData = LoadByJson<SaveData>("GameData");
        LoadData(saveData);
    }
}
