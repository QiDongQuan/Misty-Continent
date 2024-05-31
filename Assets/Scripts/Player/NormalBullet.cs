using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public GameObject creator;
    public float aliveTime = 10;
    public float speed = 16;

    private void Update()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss") || collision.CompareTag("Enemy"))
        {
            DamageInfo info = new DamageInfo(creator,collision.gameObject,creator.GetComponent<Character>().Attack);
            DamageManager.Instance.SubmitDamage(info);
            Destroy(gameObject);
        }
    }
}
