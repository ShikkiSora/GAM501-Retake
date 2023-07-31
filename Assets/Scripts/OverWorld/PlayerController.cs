using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Player Stats")]
    public float PlayerSpeed = 0.01f;
    public float PlayerHealth = 0f;
    public float PlayerMaxHealth = 1f;
    public float LevelTimer = 1f;

    public GameObject HealthPack;
    
    [Header("Textbox")]
    public Image FillImage;
    public Image BackGroundImage;
    public bool DialogueActive;

    Animator animator;
    SpriteRenderer spriteRenderer;
    public Transform SpawnPoint,Player;

    private void Awake()
    {
        instance = this;
        PlayerPrefs.SetFloat("playerXPosition", SpawnPoint.position.x);
        PlayerPrefs.SetFloat("playerYPosition", SpawnPoint.position.y);
    }

    void Start()
    {
        LoadPosition();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Timer());
        DialogueActive = false;
        BackGroundImage.enabled = false;
        print(UIManager.Instance);
        //UIManager.Instance.DialogueText.enabled = false;    
    }

    void Update()
    {
        InventorySystem();
        HealthSystem();

        // If the piano exists...
        if (PianoControler.instance != null)
        {

            // but it's not being used...
            bool isPianoBeingUsed = PianoControler.instance.UsingPiano;
            if (!isPianoBeingUsed){
                // Update the movement.
                UpdateMovement();
            }
            
        } else { // if piano doesn't exist update movement
            UpdateMovement();
        }        
    }

    public void UpdateMovement()
    {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            transform.Translate(0, y * PlayerSpeed * Time.deltaTime, 0);
            transform.Translate(x * PlayerSpeed * Time.deltaTime, 0, 0);
            //FillImage.fillAmount = PlayerHealth / PlayerMaxHealth;
            
            if (Mathf.Abs(x) > 0.01 && Mathf.Abs(y) > 0.01){
                PlayerStartWalking();
            }

            // Set animator / sprite
            animator.SetFloat("xSpeed", Mathf.Abs(x)); // X is horizontal input
            animator.SetFloat("ySpeed", y); // Y is vertical input

            // Won't change value if y = 0, won't reset value if no input is pressed
            if (y < 0)
            {
                animator.SetBool("goingUp", false);
            }
            else if (y > 0)
            {
                animator.SetBool("goingUp", true);

            }


            if (x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (x > 0)
            {
                spriteRenderer.flipX = false;
            }  
    }

    public void PlayerStartWalking() {}



    IEnumerator Timer()
    {
       while(LevelTimer <=1000)
        {
            LevelTimer++;
            yield return new WaitForSeconds(1);
           // Debug.Log(LevelTimer);
        }
    }
    public void HealthSystem()
    {
        if (PlayerHealth == PlayerMaxHealth)
        {
            Time.timeScale = 0;
        }
        else
        {
            //Time.timeScale = 1;
        }
    }
    public void InventorySystem()
    {
        if (DialogueActive == true)
        {
            BackGroundImage.enabled = true;
            UIManager.Instance.DialogueText.enabled = true;
        }
        else
        {
            BackGroundImage.enabled = false;
            UIManager.Instance.DialogueText.enabled = false;
        }
    }
    public void DialogueSystem()
    {

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Health")
        {
            HealthPack.transform.position = new Vector2(1000, 1000);
            PlayerHealth -= 0.25f;
            Debug.Log(PlayerHealth);
        }
    }

    public void OverridePlayerPosition(Vector2 position_)
    {
        PlayerPrefs.SetString("isOverridePlayerPosition", "True");
        PlayerPrefs.SetFloat("playerXPosition", position_.x);
        PlayerPrefs.SetFloat("playerYPosition", position_.y);
        PlayerPrefs.Save();
    }

    public void SetPosition()
    {
        if (PlayerPrefs.GetString("isOverridePlayerPosition") == "True")
        {
            PlayerPrefs.SetFloat("playerXPosition", transform.position.x);
            PlayerPrefs.SetFloat("playerYPosition", transform.position.y);
            PlayerPrefs.Save();
        } else {
            print("PLAYER POSITION OVERRIDDEN");
        }
    }

    public void LoadPosition()
    {
        Vector3 newPlayerPosition = new Vector3(
            PlayerPrefs.GetFloat("playerXPosition"),
            PlayerPrefs.GetFloat("playerYPosition"),
           0f
        );

        PlayerPrefs.SetString("isOverridePlayerPosition", "True");

        transform.position = newPlayerPosition;
    }

    public void SetEnemyName(string name)
    {
        PlayerPrefs.SetString("enemyGameObjectName", name);
        PlayerPrefs.Save();
    }

    public GameObject LoadEnemyName()
    {
        return GameObject.Find(PlayerPrefs.GetString("enemyGameObjectName"));
    }

    public void HasProcessedResult(bool hasProcessedResult)
    {
        PlayerPrefs.SetString("hasProcessedResult", bool.TrueString);
    }

    public void ProcessResult()
    {
        if (PlayerPrefs.GetString("hasProcessedResult") == "True")
        {
            HasProcessedResult(true);
            if (PlayerPrefs.GetString("isEnemyDefeated") == "True")
            {
                LoadEnemyName().SetActive(false);
            }
        }
    }
}
