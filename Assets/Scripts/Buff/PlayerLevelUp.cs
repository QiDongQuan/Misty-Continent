using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevelUp_BM", menuName = "BuffSystem/PlayerLevelUp_BM", order = 3)]
public class PlayerLevelUp : BaseBuffModule
{
    public override void Applay(BuffInfo buffInfo, DamageInfo damageInfo = null)
    {
        Debug.Log("经验增加");
        buffInfo.target.GetComponent<PlayerCharacter>().experience += 20;
    }
}
