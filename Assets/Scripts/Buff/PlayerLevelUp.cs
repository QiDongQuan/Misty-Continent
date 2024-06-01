using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevelUp_BM", menuName = "BuffSystem/PlayerLevelUp_BM", order = 3)]
public class PlayerLevelUp : BaseBuffModule
{
    public override void Applay(BuffInfo buffInfo, DamageInfo damageInfo = null)
    {
        PlayerCharacter player = buffInfo.target?.GetComponent<PlayerCharacter>();
        Character enemy = damageInfo.target?.GetComponent<Character>();
        if (player)
        {
            Debug.Log($"{player.name}击杀{enemy.name}获得{enemy.DropExperience}经验");
            player.experience += enemy.DropExperience;
        }
    }
}
