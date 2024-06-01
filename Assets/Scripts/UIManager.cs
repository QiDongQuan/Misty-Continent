using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public Image itemIcon;//����ͼ��Ԥ����
    public Transform grid;//����ͼ����Ӹ�����
    public Transform playerGrid;//���װ���۵���ͼ����Ӹ�����
    public Transform infoPanel;//������Ϣ���
    public RectTransform rect;//������ͼ
    [HideInInspector]
    public Transform from;//����϶�����ͼ��ʱ��¼���Ǵ������ϳ�����

    PlayerCharacter player;
    public RectTransform Canvas;
    public Camera UICamera;

    public Text Tips;
    public Image ExperienceBar;
    public Text ExperienceShow;
    public Text Lv;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }
    private void Update()
    {
        LvRefresh();
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

    //SetItem���ؼ���_grid������Ҫˢ�µ�λ�ñ�������װ����
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
        infoPanel.transform.GetChild(1).GetComponent<Text>().text = "Ʒ��:" + item.quality;
        infoPanel.transform.GetChild(2).GetComponent<Text>().text = "������:" + item.jsonData.Attack[item.quality - 1];
        infoPanel.transform.GetChild(3).GetComponent<Text>().text = "����:" + item.jsonData.Defensive[item.quality - 1];
        infoPanel.transform.GetChild(4).GetComponent<Text>().text = "����ֵ:" + item.jsonData.Hp[item.quality - 1];
        infoPanel.transform.GetChild(4).GetComponent<Text>().text = "����:";
    }

    public void HideInfoPanel()
    {
        infoPanel.gameObject.SetActive(false);
    }

    public void IconDrag(PointerEventData eventData, ItemData item)
    {
        //���������ͼ���ϳ��˱�����ͼ��Χ���ӱ�����ɾ������
        if (!RectTransformUtility.RectangleContainsScreenPoint(rect, eventData.position, UICamera))
        {
            player.GetComponent<Bag>().RemoveItem(item, from);
            HideInfoPanel();
            return;
        }
        else
        {
            //�������е��߿����������ĳ�����߿����Ŀ����߿򽻻�λ��
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
            //���������߱������˱�����ͼ�Ŀ���λ�ã���Ҫ��ԭ����ԭ��λ�õ�ͼ��
            player.GetComponent<Bag>().RestoreItem(item, from);
            HideInfoPanel();
        }
    }
}
