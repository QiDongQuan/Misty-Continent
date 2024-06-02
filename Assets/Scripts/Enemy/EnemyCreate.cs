using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCreate : MonoBehaviour
{
    List<Transform> enemyBornPoint;
    List<Transform> bossBornPoint;

    private void Start()
    {
        enemyBornPoint = new List<Transform>();
        bossBornPoint = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("EnemyBorn"))
            {
                enemyBornPoint.Add(transform.GetChild(i));
            }else if (transform.GetChild(i).CompareTag("BossBorn"))
            {
                bossBornPoint.Add(transform.GetChild(i));
            }
        }

        CreateEnemy();
    }

    void CreateEnemy()
    {
        int MapId = PlayerPrefs.GetInt("MapId",1001);
        MapEnemiesData data =  MapJsonManager.Instance.GetJson(MapId);
        if(data == null)
        {
            Debug.Log($"id为{MapId}的地图配置为空");
            return;
        }

        int allCount = 0;
        for (int i = 0;i<data.Enemies.Count;i++)
        {
            int index = data.Enemies[i].Count;
            while(index > 0)
            {
                EnemyCharacter tmp = Instantiate(Resources.Load<EnemyCharacter>(EnemyJsonManager.Instance.GetJson(data.Enemies[i].ID).PrefabPath), enemyBornPoint[allCount%enemyBornPoint.Count]);
                tmp.Attack = data.Enemies[i].Attack;
                tmp.Armor = data.Enemies[i].Armor;
                tmp.Hp = data.Enemies[i].Hp;
                tmp.dropList = CreateItem(data.Enemies[i].DropID);
                Vector3 point = tmp.transform.parent.position;
                tmp.transform.position = new Vector3(point.x+Random.Range(0,3), point.y + Random.Range(0, 3), point.z);
                tmp.gameObject.SetActive(false);
                index--;
                allCount++;
            }
        }
        allCount = 0;
        for (int i = 0; i < data.Bosses.Count; i++)
        {
            int index = data.Bosses[i].Count;
            while (index > 0)
            {
                BossCharacter tmp = Instantiate(Resources.Load<BossCharacter>(EnemyJsonManager.Instance.GetJson(data.Bosses[i].ID).PrefabPath), bossBornPoint[allCount % bossBornPoint.Count]);
                tmp.Attack = data.Bosses[i].Attack;
                tmp.Armor = data.Bosses[i].Armor;
                tmp.Hp = data.Bosses[i].Hp;
                tmp.dropList = CreateItem(data.Bosses[i].DropID);
                Vector3 point = tmp.transform.parent.position;
                tmp.transform.position = new Vector3(point.x + Random.Range(0, 3), point.y + Random.Range(0, 3), point.z);
                tmp.gameObject.SetActive(false);
                index--;
                allCount++;
            }
        }
    }

    List<ItemData> CreateItem(int[] arr)
    {
        List<ItemData> items = new List<ItemData>();
        for (int i = 0;i< arr.Length;i++)
        {
            items.Add(WorldItemManager.Instance.CreateItem(arr[i]));
        }
        return items;
    }
}
