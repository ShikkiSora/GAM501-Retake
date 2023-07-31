using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    public int currentxp = 0;
    public int targetxp = 50;
    public int playerlvl = 1;
    public Text currentxptext;
    public bool Levelup;

    // Start is called before the first frame update
    void Start()
    {
        Levelup = false;
    }

    // Update is called once per frame

    public void Update()
    {
        XpScript();
        currentxptext.text = currentxp.ToString() + "/" + targetxp.ToString();
    }





    void XpScript()
    {


        if (Input.GetKeyDown(KeyCode.W))
        {
            currentxp += 20;
        }

        if (currentxp >= targetxp)
        {
            playerlvl += 1;
            Levelup = true;
            Debug.Log(playerlvl);
        }
        if (Levelup == true)
        {
            Levelup = false;
            currentxp -= targetxp;
            targetxp += 50;
        }
    }






}
