using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuffModule : ScriptableObject
{
    public abstract void Applay(BuffInfo buffInfo, DamageInfo damageInfo = null);
}
