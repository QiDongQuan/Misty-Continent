using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DamageInfo info = new DamageInfo(transform.parent.gameObject, collision.gameObject, transform.parent.GetComponent<Character>().Attack);
            DamageManager.Instance.SubmitDamage(info);
        }
    }
}
