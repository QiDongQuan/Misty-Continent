using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float aliveTime = 5f;
    [HideInInspector]
    public GameObject creator;

    private void Update()
    {
        aliveTime -= Time.deltaTime;
        if(aliveTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DamageInfo info = new DamageInfo(creator,collision.gameObject,creator.GetComponent<Character>().Attack);
            DamageManager.Instance.SubmitDamage(info);
        }
    }
}
