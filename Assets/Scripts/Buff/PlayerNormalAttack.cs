using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNormalAttack_BM", menuName = "BuffSystem/PlayerNormalAttack_BM", order = 1)]
public class PlayerNormalAttack : BaseBuffModule
{
    public GameObject bullet;

    public override void Applay(BuffInfo buffInfo, DamageInfo damageInfo = null)
    {
        PlayerCharacter playerCharacter = buffInfo.creator.GetComponent<PlayerCharacter>();
        if(playerCharacter.target != null )
        {
            GameObject tmp = Instantiate(bullet);
            tmp.transform.position = playerCharacter.firePoint.position;
            tmp.transform.right = tmp.transform.position - buffInfo.target.transform.position;
            NormalBullet normalBullet = tmp?.GetComponent<NormalBullet>();
            if (normalBullet)
            {
                normalBullet.creator = buffInfo.creator;
            }
        }
    }
}
