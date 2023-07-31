using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BattleCharacterManager : MonoBehaviour
{

    public GameObject inGameEnemyObject;
    public TextMeshProUGUI enemyText;

    public List<string> enemyName;
    public List<GameObject> enemyGameObject;

    public void LoadEnemyObject()
    {
        string enemyName_ = PlayerPrefs.GetString("EnemyType");
        enemyText.text = enemyName_.ToUpper();
        
        print(enemyName_.ToUpper());
        int enemyIndex = enemyName.IndexOf(enemyName_.ToUpper());
        print(enemyIndex);

        GameObject enemyGameObjectInstantiated = Instantiate(enemyGameObject[enemyIndex]);

        enemyGameObjectInstantiated.GetComponent<EnemyAi>().enabled = false;
        enemyGameObjectInstantiated.GetComponent<SpriteRenderer>().sortingOrder = 0;

        enemyGameObjectInstantiated.transform.SetParent(inGameEnemyObject.transform);
        enemyGameObjectInstantiated.transform.localPosition = new Vector3(0, -2, 0);
        enemyGameObjectInstantiated.transform.localScale = new Vector3(2f, 2f, 2f);
    }

    void Start()
    {
        LoadEnemyObject();
    }
}
