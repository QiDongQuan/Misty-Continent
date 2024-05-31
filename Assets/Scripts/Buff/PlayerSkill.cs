using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkill_BM", menuName = "BuffSystem/PlayerSkill_BM", order = 2)]
public class PlayerSkill : BaseBuffModule
{
    public GameObject skillPrefab;

    public override void Applay(BuffInfo buffInfo, DamageInfo damageInfo = null)
    {
        PlayerCharacter playerCharacter = buffInfo.creator.GetComponent<PlayerCharacter>();
        if (buffInfo.buffData.skillExpend > playerCharacter.Enenrgy)
        {
            return;
        }
        if (playerCharacter.target != null)
        {
            GameObject tmp = Instantiate(skillPrefab);
        }
        playerCharacter.Enenrgy -= buffInfo.buffData.skillExpend;
    }
}
