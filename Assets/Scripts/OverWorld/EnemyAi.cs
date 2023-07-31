using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;

// Stopped enemy disappearing glitch
// Made text dynamic

public class EnemyAi : MonoBehaviour
{
    //public float DotProduce;

    public Color highlightedColor = new Color(1f, 0.8f, 0.8f, 1f);

    //public Vector2 LocalForwardVector;
    //public Vector2 VectorBetweenPlayerAndEnemy;
    //public float DistanceBetweenPlayerAndEnemy;
    public float EnemySpeed = 1f;
    public List<string> EnemyText;

    public EnemyGameObjectName enemyType;
    
    GameObject Character;
    int currentTextIndex;
    bool isPlayerInTrigger;

    PlayerController playerController;

    [Header("Movement")]
    public float speed;
    public List<Vector2> wayPoints;
    public float pauseTime; // Time to wait after each waypoint

    bool movementStateMachineActive = true;
    MovementCurrentState movementCurrentState = MovementCurrentState.MOVING;
    int currentWaypointIndex = 0;
    float waitTimer = 0f;

    public GameManager gameManager;

    //public bool isInTalkingRangeBeforeUpdated;
    //public bool isInTalkingRangeAfterUpdated;

    //public bool playerHasEntered;
    //public bool playerHasExited;

    SpriteRenderer sr;

    public enum MovementCurrentState
    {
        MOVING,
        WAITING
    }

    public enum EnemyGameObjectName
    {
        GHOST,
        GOBLIN,
        GOLEM,
        HARPY,
        IMP,
        SKELETON,
        SPIDER
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentTextIndex = -1;
        //LocalForwardVector = transform.forward;
        sr = GetComponent<SpriteRenderer>();
        Character = GameObject.Find("PlayerCharacter");
        playerController = Character.GetComponent<PlayerController>();
    }



    // Update is called once per frame
    void Update()
    {
        // STATE MACHINE START
        if (movementStateMachineActive) UpdateStateMachine();




        // Dot product solution:
        /*
        DistanceBetweenPlayerAndEnemy = Vector2.Distance(transform.position, Character.transform.position);

        // Will be updated in LateUpdate to make an OnTriggerEnter and OnTriggerExit effect
        isInTalkingRangeBeforeUpdated = (DotProduce >= 0.8) && (DistanceBetweenPlayerAndEnemy < 5);



        

        DotPro();

        // A bit more than diagonal + leass than 5



        // These are like OnTriggerEnter and OnTriggerExit but I figure it out myself
        playerHasEntered = isInTalkingRangeBeforeUpdated && !isInTalkingRangeAfterUpdated;
        playerHasExited = !isInTalkingRangeBeforeUpdated && isInTalkingRangeAfterUpdated;

        // If the player's entered the text display range and the text isn't active yet, activate it.
        if (playerHasEntered)
        {
            

        }
        // If they've left the text display range then hide it and reset the current text index.
        else if (playerHasExited)
        {
            
            currentTextIndex = 0;
        }
        // Otheriwse, if they're in the area and they press space, advance the text.
        else if (isInTalkingRangeBeforeUpdated && Input.GetKeyDown(KeyCode.Space))
        {
            print("NextTextRequested");
            // If they've reached the end of the text then hide it after they press space at the last text.
            if (currentTextIndex > EnemyText.Count)
            {
                print("At final text");
                HideText();
            }
            else
            {
                print("Next Text");
                currentTextIndex++;
                ShowText();

            }
        
        }


        //isInTalkingRangeAfterUpdated = (DotProduce >= 0.8) && (DistanceBetweenPlayerAndEnemy < 5);

        */

        
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            print("Space pressed");
            // If the player is not at the final textbox of the enemy...
            if (currentTextIndex < EnemyText.Count - 1)
            {
                print("Next");
                // Increase the textbox index, and update the text...
                if (UIManager.Instance.gameObject.active) currentTextIndex++;
                ShowText();
                // And disable the enemy movement.
                movementStateMachineActive = false;
            }
            else // If the player is at the final textbox of the enemy...
            {
                print("Hidden");
                // Hide the textbox, reset the textbox index, and reactivate the enemy movement.
                HideText();
                currentTextIndex = -1;
                movementStateMachineActive = true;
                SetInGameEnemyName(enemyType);
                playerController.SetPosition();
                playerController.SetEnemyName(gameObject.name);
                playerController.HasProcessedResult(false);
                LoadingBattleScene();
                gameManager.LoadScene(6);
            }
        }


    }

    public void LoadingBattleScene() {}

    void UpdateStateMachine()
    {
        switch (movementCurrentState)
        {
            case MovementCurrentState.MOVING:

                Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
                Vector2 targetWaypoint = wayPoints[currentWaypointIndex];
                // If close waypoint, increase waypoint index and switch movementCurrentState to WAITING, and set the timer.
                
                if (Mathf.Abs((currentPosition - targetWaypoint).magnitude) < 0.1f)
                {
                    if (currentWaypointIndex == wayPoints.Count - 1)
                    {
                        currentWaypointIndex = 0;
                    }
                    else
                    {
                        currentWaypointIndex++;
                    }
                    waitTimer = pauseTime;
                    
                    movementCurrentState = MovementCurrentState.WAITING;
                }
                else
                // Otherwise, move towards next waypoint
                {
                    Vector2 vectorTowardsTarget = (targetWaypoint - currentPosition).normalized * Time.deltaTime * speed;
                    transform.position += new Vector3(vectorTowardsTarget.x, vectorTowardsTarget.y, 0f);
                }
                break;

            case MovementCurrentState.WAITING:
                // Make a timer manually instead of an IEnumerator so that it can be paused.
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f) movementCurrentState = MovementCurrentState.MOVING; // Once timer has hit zero, switch back to the moving state.
                break;
        }
    }

    public void SetInGameEnemyName(EnemyGameObjectName enemyType)
    {
        PlayerPrefs.SetString("EnemyType", enemyType.ToString());
        print(enemyType.ToString());
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollidedWithEnemy();
            print("Entered");
            isPlayerInTrigger = true;
            sr.color = highlightedColor;
        }

    }

    public void CollidedWithEnemy() {}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Exited");
            HideText();
            currentTextIndex = -1;
            isPlayerInTrigger = false;
            sr.color = Color.white;

            movementStateMachineActive = true;
        }
    }

    void ShowText()
    {
        // Shows text based on currentTextIndex
        print("ShowText " + gameObject.name);
        UIManager.Instance.DialogueText.text = (EnemyText[currentTextIndex]);
        PlayerController.instance.DialogueActive = true;
        UIManager.Instance.DialogueText.enabled = true;
    }

    void HideText()
    {
        // Hides text
        print("HideText" + gameObject.name);
        PlayerController.instance.DialogueActive = false;
        UIManager.Instance.DialogueText.enabled = false;
    }
    /*
    void DotPro()
    {
        LocalForwardVector = transform.up;
        VectorBetweenPlayerAndEnemy = PlayerController.instance.transform.position - transform.position;
        VectorBetweenPlayerAndEnemy.Normalize();
        DotProduce = Vector2.Dot(LocalForwardVector, VectorBetweenPlayerAndEnemy);
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(DotProduce);
        }
    }
    */
}