using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public Image itemIcon;//道具图标预制体
    public Transform grid;//道具图标格子父物体
    public Transform playerGrid;//玩家装备槽道具图标格子父物体
    public Transform infoPanel;//道具信息面板
    public RectTransform rect;//背包底图
    [HideInInspector]
    public Transform from;//鼠标拖动道具图标时记录，是从哪里拖出来的

    PlayerCharacter player;
    public RectTransform Canvas;
    public Camera UICamera;

    public Text Tips;
    public Image ExperienceBar;
    public Text ExperienceShow;
    public Text Lv;
    public Text PlayerAttack;
    public Text PlayerArmor;
    public Text PlayerHp;
    public Text Successful;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }
    private void Update()
    {
        LvRefresh();
        PlayerAttack.text = "攻击力：" + player.Attack.ToString();
        PlayerArmor.text = "护甲：" + player.Armor.ToString();
        PlayerHp.text = "生命值" + player.MaxHp.ToString();
    }

    public void ShowRecived()
    {
        Successful.rectTransform.localScale = Vector3.zero;
        Successful.gameObject.SetActive(true);
        Color c = Successful.color;
        Successful.color = new Color(c.r, c.g, c.b);

        Sequence seq = DOTween.Sequence();//动画序列
        seq.Append(Successful.rectTransform.DOScale(1,1f));
        seq.AppendInterval(1f);//等待0.8秒
        seq.AppendCallback(() =>//序列结束时执行的回调函数
        {
            Successful.gameObject.SetActive(false);
        });
    }

    public void LvRefresh()
    {
        ExperienceBar.fillAmount = player.experience / (100.0f + (player.lv - 1) * 25.0f);
        if (player.experience >= (100 + (player.lv - 1) * 25))
        {
            player.experience -= (100 + (player.lv - 1) * 25);
            player.lv += 1;
            Lv.text = player.lv.ToString();
        }
        ExperienceShow.text = (ExperienceBar.fillAmount*100).ToString()+"%";
    }

    public void SetTips(string text)
    {
        Tips.text = text;
        Tips.gameObject.SetActive(true);
    }

    public void SetTips(string text, float time)
    {
        Tips.text = text;
        Tips.gameObject.SetActive(true);
        StartCoroutine(OffTips(time));
    }

    IEnumerator OffTips(float time)
    {
        yield return new WaitForSeconds(time);
        Tips.gameObject.SetActive(false);
    }

    public void SetItem(int index, ItemData item)
    {
        Transform solt = grid.GetChild(index);

        if (solt.childCount > 0)
        {
            Destroy(solt.GetChild(0).gameObject);
        }

        if (item != null)
        {
            Image image = Instantiate(itemIcon, solt);
            image.GetComponent<UIItem>().data = item;
            image.sprite = Resources.Load<Sprite>(item.jsonData.IamgePath);
        }
    }

    //SetItem重载加入_grid代表所要刷新的位置背包还是装备槽
    public void SetItem(int index, ItemData item, Transform _grid)
    {
        Transform solt = _grid.GetChild(index);

        if (solt.childCount > 0)
        {
            Destroy(solt.GetChild(0).gameObject);
        }

        if (item != null)
        {
            Image image = Instantiate(itemIcon, solt);
            image.GetComponent<UIItem>().data = item;
            image.sprite = Resources.Load<Sprite>(item.jsonData.IamgePath);
        }
    }

    public void ShowInfoPanel(ItemData item)
    {
        infoPanel.gameObject.SetActive(true);
        infoPanel.transform.GetChild(0).GetComponent<Text>().text = item.jsonData.Name;
        infoPanel.transform.GetChild(1).GetComponent<Text>().text = "品质:" + item.quality;
        infoPanel.transform.GetChild(2).GetComponent<Text>().text = "攻击力:" + item.jsonData.Attack[item.quality - 1];
        infoPanel.transform.GetChild(3).GetComponent<Text>().text = "护甲:" + item.jsonData.Defensive[item.quality - 1];
        infoPanel.transform.GetChild(4).GetComponent<Text>().text = "生命值:" + item.jsonData.Hp[item.quality - 1];
        //infoPanel.transform.GetChild(4).GetComponent<Text>().text = "技能:";
    }

    public void HideInfoPanel()
    {
        infoPanel.gameObject.SetActive(false);
    }

    public void IconDrag(PointerEventData eventData, ItemData item)
    {
        //如果将道具图标拖出了背包底图范围，从背包中删除道具
        if (!RectTransformUtility.RectangleContainsScreenPoint(rect, eventData.position, UICamera))
        {
            player.GetComponent<Bag>().RemoveItem(item, from);
            HideInfoPanel();
            return;
        }
        else
        {
            //遍历所有道具框，如果拖入了某个道具框，则和目标道具框交换位置
            for (int i = 0; i < grid.childCount; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(grid.GetChild(i).gameObject.GetComponent<RectTransform>(), eventData.position, UICamera))
                {
                    player.GetComponent<Bag>().SwapItem(item, i, player.GetComponent<Bag>().items, grid);
                    return;
                }
            }
            for (int i = 0; i < playerGrid.childCount; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(playerGrid.GetChild(i).gameObject.GetComponent<RectTransform>(), eventData.position, UICamera))
                {
                    player.GetComponent<Bag>().SwapItem(item, i, player.GetComponent<Bag>().playerItems, playerGrid);
                    return;
                }
            }
            //否则代表道具被拖入了背包底图的空余位置，需要复原道具原先位置的图标
            player.GetComponent<Bag>().RestoreItem(item, from);
            HideInfoPanel();
        }
    }
}
