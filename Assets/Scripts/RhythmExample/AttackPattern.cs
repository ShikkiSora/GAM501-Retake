using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackPattern", menuName = "ScriptableObjects/AttackPattern")]
public class AttackPattern : ScriptableObject
{
    public string attackName;
    public float damage;
    public List<KeyCode> keys = new List<KeyCode>();
    public List<float> beats = new List<float>();
}
