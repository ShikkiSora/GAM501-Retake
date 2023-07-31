using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AttackDamageCalculationDatabase : MonoBehaviour
{

    public EnemyAttackDetails enemyAttackDetails;


    public AttackManager attackManager;
    public List<AttackType> attacks;
    public List<float> accuracy;
    public List<BattleEffect> battleEffectList;
    public List<GameObject> attackButtons;
    public List<AttackPattern> attackPatterns;




    public enum AttackType
    {
        PIANOA,
        PIANOD,
        PIANOS,

        SAXA,
        SAXD,
        SAXS,

        BGUITARA,
        BGUITARD,
        BGUITARS
    }


    private void Start()
    {
        // Since ADCD and AttackManager are on the same object...
        enemyAttackDetails = GetComponent<AttackManager>().enemyAttackDetails;
    }

    public void ClearAttackList()
    {
        attacks.Clear();
        accuracy.Clear();
    }

    public void ProcessCooldowns()
    {
        foreach (GameObject attackButton_ in attackButtons)
        {
            AttackType attackButtonType = attackButton_.GetComponent<AttackButton>().attackType;
            Toggle attackButtonToggle = attackButton_.GetComponent<Toggle>();
            if (attacks.Contains(attackButtonType))
            {
                //print("CONTAINS");
                attackButtonToggle.interactable = false;
            } else
            {
                //print("DOES NOT CONTAINS");
                attackButtonToggle.interactable = true;
            }
        }
    }

    public void ResetToggles()
    {
        foreach (GameObject attackButton_ in attackButtons)
        {
            attackButton_.GetComponent<Toggle>().isOn = false;
        }
    }

    public DamageObject CalculateAllAttacks() // Feeds the damageObject into AttackManager
    {

        DamageObject damObj = DamageObject.CreateInstance<DamageObject>();


        // Add enemy attack here
        damObj.enemyAttackDamage = 0f;
        damObj.enemyAttackDamage += UnityEngine.Random.Range(enemyAttackDetails.minAttack, enemyAttackDetails.maxAttack);



        damObj.playerAttackDamage = 0f;


        print("Attacks count: " + attacks.Count.ToString());
        for (int i = 0; i < attacks.Count; i++)
        {
            List<BattleEffect> currentBattleEffects;
            damObj = CalculateDamage(attacks[i], damObj, accuracy[i], out currentBattleEffects);

            // Add the battle effects from this attack to the overall battleEffects
            battleEffectList.AddRange(currentBattleEffects);
            currentBattleEffects.Clear();

            print(string.Format("Attack {0} with {1} accuracy: Player Attack: <color=yellow>{2}</color>, Enemy Attack: <color=red>{3}</color>",
                attacks[i],
                accuracy[i],
                damObj.playerAttackDamage,
                damObj.enemyAttackDamage
            ));

        } 

        return damObj;
    }




    /* REMEMBER: Add a list of effects...
     * ...turns they will be active in
     * ...and how long they're active for
     */

    // Effects

    /*
    DamageObject RandomEnemyAttack(DamageObject damObj, float min, float max)
    {
        damObj.enemyAttackDamage = UnityEngine.Random.Range(min, max);
    }
    */

    float RoundToNearest(float numToRound, float numRoundedTo)
    {
        return (float) Math.Round(numToRound / numRoundedTo) * numRoundedTo;
    }

    DamageObject ScalePlayerDamage(DamageObject damageObject, float percent) // As decimal
    {
        damageObject.playerAttackDamage *= percent;
        return damageObject;
    }

    DamageObject ScaleEnemyDamage(DamageObject damageObject, float percent) // As decimal
    {
        damageObject.enemyAttackDamage *= percent;
        return damageObject;
    }

    DamageObject DisableStravaganza(DamageObject damageObject, float percent)
    {
        // Disable stravaganza
        return damageObject;
    }
    
    DamageObject CalculateDamage(AttackType attackType, DamageObject damageObject, float accuracy, out List<BattleEffect> outBattleEffects)
    {
        outBattleEffects = new List<BattleEffect>();
        
        switch (attackType)
        {
            case AttackType.PIANOA: 
                damageObject.playerAttackDamage += RoundToNearest(50f * accuracy, 5f);
                return damageObject;

            case AttackType.PIANOD:
                damageObject.enemyAttackDamage = 0f;

                // ...player attack is weakened for next ~5 turns
                BattleEffect weakenPlayerDamage = new BattleEffect();
                weakenPlayerDamage.effectEvents.Add(ScalePlayerDamage);
                weakenPlayerDamage.turnsLeftUntilActive = 1;
                weakenPlayerDamage.turnsActiveFor = (int)Mathf.Floor(5f * (1f - accuracy));

                outBattleEffects.Add(weakenPlayerDamage);
                return damageObject;

            case AttackType.PIANOS:

                float maximumRatio = 0.5f * accuracy;
                float initialEnemyDamage = damageObject.enemyAttackDamage;

                float newPlayerAttackDamage = RoundToNearest(maximumRatio * initialEnemyDamage, 5f);
                damageObject.playerAttackDamage += newPlayerAttackDamage;
                damageObject.enemyAttackDamage += initialEnemyDamage - newPlayerAttackDamage;
                return damageObject;


            case AttackType.SAXA: // SaxA
                damageObject.playerAttackDamage += RoundToNearest(50f * accuracy, 5f);
                return damageObject;


            case AttackType.SAXD: // SaxD
                damageObject.enemyAttackDamage -= RoundToNearest(40f * accuracy, 5f);
                return damageObject;

            case AttackType.SAXS: // SaxS

                BattleEffect disableStravaganzaBE = new BattleEffect();
                disableStravaganzaBE.effectEvents.Add(DisableStravaganza);
                disableStravaganzaBE.turnsActiveFor = (int)Mathf.Floor(5 * (1 - accuracy));
                disableStravaganzaBE.turnsLeftUntilActive = 0;

                outBattleEffects.Add(disableStravaganzaBE);
                return damageObject;


            case AttackType.BGUITARA: //BGuitarA
                damageObject.playerAttackDamage += RoundToNearest(100f * accuracy, 5f);
                // Remove enemy defense
                BattleEffect removeEnemyDefense = new BattleEffect();
                removeEnemyDefense.turnsActiveFor = 1;
                removeEnemyDefense.turnsLeftUntilActive = 0;
                outBattleEffects.Add(removeEnemyDefense);

                // Increase enemy attack
                BattleEffect increaseEnemyDefense = new BattleEffect();
                increaseEnemyDefense.turnsActiveFor = 1;
                removeEnemyDefense.turnsLeftUntilActive = 0;

                outBattleEffects.Add(increaseEnemyDefense);
                return damageObject;


            case AttackType.BGUITARD: // BGuitarD
                // ???
                return damageObject;

            case AttackType.BGUITARS: // BGuitarS
                // ???
                return damageObject;

            default:

                return damageObject;

        }
    }



}
