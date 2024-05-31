using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 5;
    public float aliveTime = 5;
    [HideInInspector]
    public GameObject creator;

    private void Update()
    {
        transform.position += transform.up * -1 * speed * Time.deltaTime;
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
            DamageInfo info = new DamageInfo(creator, collision.gameObject, creator.GetComponent<Character>().Attack);
            DamageManager.Instance.SubmitDamage(info);
            Destroy(gameObject);
        }
    }
}
