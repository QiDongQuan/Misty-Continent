using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData data;

    Camera UICamera;
    RectTransform Canvas;

    private void Start()
    {
        UICamera = UIManager.Instance.UICamera;
        Canvas = UIManager.Instance.Canvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        UIManager.Instance.from = transform.parent.parent;
        transform.SetParent(Canvas);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //因为Cancas使用了Camera模式，所以需要把屏幕鼠标输入坐标转换为指定Rectangle平面和UI摄像机的UI世界坐标
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Canvas, eventData.position, UICamera, out pos);
        transform.position = pos;//道具图标跟随鼠标移动
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UIManager.Instance.IconDrag(eventData, data);
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowInfoPanel(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideInfoPanel();
    }
}
