using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AttackManager : MonoBehaviour
{

    public GameManager gameManager;
    public AttackDamageCalculationDatabase ADCD;

    public BattleState currentBattleState;
    public bool hasExecutedCurrentState;

    public GameObject attackChooser;

    public HealthController playerHealthController;
    public HealthController enemyHealthController;

    public EnemyAttackDetails enemyAttackDetails;

    public bool finishedChoosing;

    public AttackPatternHandler attackPatternHandler;

    public GameObject successCanvas;
    public GameObject faliureCanvas;


    public enum BattleState
    {
        ENTERING, // Entering battle
        CHOOSING, // Choosing attacks
        EXECUTING, // Performing the rhythm for the attacks
        VISUALS, // Seeing the damage visuals appear etc.
        SUCCESS, // Enemy defeated
        FALIURE // Player defeated
    }

    // Start is called before the first frame update
    void Start()
    {
        // Stores enemy's range of attack and defense points
        AttackDamageCalculationDatabase ACDC = GetComponent<AttackDamageCalculationDatabase>();
        enemyAttackDetails = GameObject.Find("EnemyCharacter").GetComponent<EnemyAttackDetails>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateBattleState();
        ExecuteBattleState(currentBattleState); // Do corresponding action for current state
        //print(currentBattleState);
    }


    // Are breaks necessary? This only allows one state change per frame, when there may have to be multiple
    /*
    void UpdateBattleState() 
    {
        switch (currentBattleState)
        {
            case (BattleState.ENTERING):
                // CHOOSING (time delay?)
                currentBattleState = BattleState.CHOOSING;
                break;

            case (BattleState.CHOOSING):
                // EXECUTING
                if (finishedChoosing)
                {
                    finishedChoosing = false;
                    currentBattleState = BattleState.EXECUTING;
                }
                break;

            case (BattleState.EXECUTING):
                // VISUALS
                break;
            case (BattleState.VISUALS):
                // CHOOSING
                // SUCCESS
                // FALIURE
                break;
            case (BattleState.SUCCESS):
                // goto menu or smth (good)
                break;
            case (BattleState.FALIURE):
                // goto menu or smth (bad)
                break;
        }
    }
    */

    void ExecuteBattleState(BattleState battleState)
    {
        if (!hasExecutedCurrentState)
        {
            print(currentBattleState);
            switch (currentBattleState)
            {
                case (BattleState.ENTERING):
                    ADCD.ClearAttackList();
                    StartCoroutine(StartEnteringDelay()); // Delay
                    break;

                case (BattleState.CHOOSING):
                    
                    attackChooser.SetActive(true);
                    break;

                case (BattleState.VISUALS):
                    StartCoroutine(WaitForRhythmToFinish());
                    break;

                case (BattleState.SUCCESS):
                    successCanvas.SetActive(true);
                    PlayerPrefs.SetString("isEnemyDefeated", "True");
                    PlayerPrefs.Save();

                    StartCoroutine(LoadSceneAfterSeconds(1, 2f));
                    break;

                case (BattleState.FALIURE):
                    faliureCanvas.SetActive(true);
                    PlayerPrefs.SetString("isEnemyDefeated", "False");
                    PlayerPrefs.Save();

                    StartCoroutine(LoadSceneAfterSeconds(1, 2f));
                    break;
            }
        }
    }

    // ENTERING -> CHOOSING
    IEnumerator StartEnteringDelay()
    {
        hasExecutedCurrentState = true;
        yield return new WaitForSeconds(1f);
        currentBattleState = BattleState.CHOOSING;
        hasExecutedCurrentState = false;
    }

    // CHOOSING -> VISUALS
    public void FinishedChoosing()
    {
        hasExecutedCurrentState = true;
        attackChooser.SetActive(false);
        currentBattleState = BattleState.VISUALS;
        hasExecutedCurrentState = false;
    }

    // VISUALS -> EXECUTING
    public void ApplyAttacks()
    {
        hasExecutedCurrentState = true;
        DamageObject damObj = ADCD.CalculateAllAttacks();
        print("PRINTING");
        ProcessDamageObject(damObj);
        currentBattleState = BattleState.CHOOSING;
        hasExecutedCurrentState = false;

    }

    public IEnumerator WaitForRhythmToFinish()
    {
        hasExecutedCurrentState = true;
        List<GameObject> allKeyObjectsOnScreen = attackPatternHandler.keyObjects;
        if (allKeyObjectsOnScreen.Count != 0)
        {
            float finalBeatToWaitFor = allKeyObjectsOnScreen[allKeyObjectsOnScreen.Count - 1].GetComponent<KeyObject>().beatOfThisKey;
            yield return new WaitUntil(() => attackPatternHandler.timeKeeper.beatsDiscrete >= finalBeatToWaitFor);
            attackPatternHandler.ClearKeyObjects();
            ApplyAttacks();
            ADCD.ResetToggles();
        }

        
        // Check where health is at and change state depending
        
        if (playerHealthController.health < 1f)
        {
            currentBattleState = BattleState.FALIURE;
        }
        else if (enemyHealthController.health < 1f)
        {
            currentBattleState = BattleState.SUCCESS;
        } else
        {
            currentBattleState = BattleState.CHOOSING;
        }
        hasExecutedCurrentState = false;
    }


    // VISUALS -> CHOOSING
    public IEnumerator ShowVisuals()
    {
        hasExecutedCurrentState = true;
        yield return new WaitForSeconds(1f);
        currentBattleState = BattleState.CHOOSING;
        hasExecutedCurrentState = false;
    }

    public void ProcessDamageObject(DamageObject damageObject)
    {
        print("processed");
        enemyHealthController.ChangeHealthAmount(-damageObject.playerAttackDamage);
        playerHealthController.ChangeHealthAmount(-damageObject.enemyAttackDamage);
    }

    public void CreateAttacksOnStaff()
    {
        List<AttackDamageCalculationDatabase.AttackType> attacks = ADCD.attacks; // Names of attack
        List<AttackPattern> attackPatterns = ADCD.attackPatterns; // All rhythmes of attack

        for (int i = 0; i < attacks.Count; i++)
        {
            print(i);
            print(attacks[i]);
            print((int)attacks[i]);
            print(attackPatterns[(int)attacks[i]]);

            attackPatternHandler.CreateAttackPatternOnNextBar(attackPatterns[(int) attacks[i]], i+1);
        }

        

    }

    public IEnumerator LoadSceneAfterSeconds(int sceneIndex, float time)
    {
        yield return new WaitForSeconds(time);
        gameManager.LoadScene(sceneIndex);
    }



}
