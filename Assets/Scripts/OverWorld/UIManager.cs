using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Text DialogueText;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

}
