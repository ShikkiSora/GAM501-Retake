using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AttackMove", menuName = "ScriptableObjects/AttackMove")]
public class AttackMove : ScriptableObject
{
    public string instrument;
    public string attackName;
    public string description;
    public AttackPattern attackPattern;
    public string type; // Attack, Def, Special
    public UnityEvent DamageCalculation;
    public int numberOfTurnsEffectLastsFor;
    public int maxDamage;



}
    