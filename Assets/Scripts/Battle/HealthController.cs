using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    public float health;
    public float maxHealth;
    public Slider healthBar;
    public TextMeshProUGUI healthText;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void ChangeHealthAmount(float amount)
    {
        if (health - amount > 0)
        {
            health += amount;
        }
        else
        {
            health = 0;
        }
        
        UpdateUI();

    }

    public void UpdateUI()
    {
        print("health: " + health.ToString());
        print("maxHealth: " + maxHealth.ToString());
        print(health / maxHealth);
        healthBar.value = health / maxHealth;
        print("update ui: "+ healthBar.value.ToString());
    }

}
