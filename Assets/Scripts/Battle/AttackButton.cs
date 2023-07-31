using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public AttackDamageCalculationDatabase.AttackType attackType;

    Toggle toggle;
    AttackManager attackManager_;
    AttackDamageCalculationDatabase attackDatabase;
    

    private void Start()
    {
        attackManager_ = GameObject.Find("AttackManager").GetComponent<AttackManager>();
        attackDatabase = GameObject.Find("AttackManager").GetComponent<AttackDamageCalculationDatabase>();
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(AddAttack);
        toggle.onValueChanged.AddListener(ButtonChosen);
        
    }

    public void ButtonChosen(bool _){}

    void AddAttack(bool toggleValue)
    {

        if (attackManager_.currentBattleState == AttackManager.BattleState.CHOOSING)
        {
            if (toggleValue)
            {
                attackDatabase.attacks.Add(attackType);
                attackDatabase.accuracy.Add(1f);
            } else
            {
                attackDatabase.attacks.Remove(attackType);
                attackDatabase.accuracy.Remove(1f); // Remove based on index
            }
        }
    }

    

}
