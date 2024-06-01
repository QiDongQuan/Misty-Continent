using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GetEnergy_BM", menuName = "BuffSystem/GetEnergy_BM", order = 5)]
public class GetEnergy : BaseBuffModule
{
    public override void Applay(BuffInfo buffInfo, DamageInfo damageInfo = null)
    {
        Character player = buffInfo.target?.GetComponent<PlayerCharacter>();
        Character enemy = damageInfo.target?.GetComponent<Character>();
        if (player)
        {
            Debug.Log($"{player.name}����{enemy.name}���{enemy.DropEnergy}����");
            player.Enenrgy += enemy.DropEnergy;
            if(player.Enenrgy > player.MaxEnenrgy)
            {
                player.Enenrgy = player.MaxEnenrgy;
            }
        }
    }
}
