using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float aliveTime = 5f;

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
            Debug.Log("Íæ¼ÒÊÜµ½ÉËº¦");
        }
    }
}
