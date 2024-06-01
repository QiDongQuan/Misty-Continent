using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [HideInInspector]
    public ItemData item;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = Resources.Load<Sprite>(item.jsonData.IamgePath);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Bag>().AddItem(item);
            Destroy(gameObject);
        }
    }
}
