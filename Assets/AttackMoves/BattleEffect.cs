using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class BattleEffect : ScriptableObject
{

    public List<Func<DamageObject, float, DamageObject>> effectEvents;
    public int turnsLeftUntilActive;
    public int turnsActiveFor;

}
