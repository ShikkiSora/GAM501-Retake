using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyObject : MonoBehaviour
{

    [Header("General Data")]
    public TextMeshProUGUI keyDisplay;
    public bool isKeyObject = true;
    public Color color;
    public SpriteRenderer spriteRenderer;
    public int orderInList; // What position this keyObject is in in the AttackPatternHandler list of keyObjects 

    [Header("Key Data")]
    public float beatOfThisKey;
    public KeyCode key;
    public float offset;
    public float lerpPosition;
    public float lerpHitbox; // What area of the lerp the hitbox can be hit

    [Header("Object Anchors")]
    public Transform spawner;
    public Transform target;
    public float spawnTargetBeats;

    bool isInPlayingArea;

    // Start is called before the first frame update
    void Start()
    {
        isInPlayingArea = false;
        HideAll();

        color = Color.grey;
        if (isKeyObject) keyDisplay.text = key.ToString();
    }

    public void UpdatePosition(float currentBeatOfMusic)
    {
        // Calculate position given the variables, and the time being fed in from AttackPattern
        // Thank god the equation I came up with unravelled itself into something nice

        lerpPosition = (currentBeatOfMusic - (beatOfThisKey + offset)) / spawnTargetBeats + 1;
        //if (isKeyObject) keyDisplay.text = (lerpPosition).ToString();
        
        // If it's inside...
        if (isInPlayingArea)
        {
            // ...going out, it's not in the playing area.
            if (0 >= lerpPosition || lerpPosition >= 1.5f)
            {
                isInPlayingArea = false;
                HideAll();
            }
        }
        else
        // If it's outside...
        {
            // ...coming in, it's in the playing area.
            if (-1f <= lerpPosition && lerpPosition <= 1.5f)
            {
                isInPlayingArea = true;
                ShowAll();
            }
        }


        transform.position = Vector3.LerpUnclamped(spawner.position, target.position, lerpPosition);
    }

    public void ShowAll()
    {
        spriteRenderer.enabled = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void HideAll()
    {
        spriteRenderer.enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if (-lerpHitbox < (lerpPosition - 1) && (lerpPosition - 1) < lerpHitbox)
        {
            if (Input.GetKeyDown(key))
            {
                KeyPressed();
                HideAll();
                print((lerpPosition - 1) / lerpHitbox);
            }
        }
    }

    public void KeyPressed() {}


}
