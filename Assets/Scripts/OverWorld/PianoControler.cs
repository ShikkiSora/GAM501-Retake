using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PianoControler : MonoBehaviour
{
    public enum KeyState { KeyOne,KeyTwo,KeyThree }
    public KeyState CurrentState;
    public Transform Player;
    public RawImage KeyBoard;
    public Texture animOne, animTwo, animThree, animFour, animFive, animSix, animSeven, animEight;
    public bool UsingPiano;
    public SpriteRenderer DoorRend;
    public BoxCollider2D DoorCol;
    public static PianoControler instance;
    void Start()
    {
        instance = this;    
        UsingPiano = false;
        CurrentState = KeyState.KeyOne;
    }
    public void ChangeState(KeyState State)
    {
        CurrentState = State;
    }

    void Update()
    {
        PoorProgramming();
        if (Vector3.Distance(transform.position, Player.position) < 5)
        {
            switch (CurrentState)
            {
                case KeyState.KeyOne:
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        ChangeState(KeyState.KeyTwo);
                    }
                    break;

                case KeyState.KeyTwo:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ChangeState(KeyState.KeyThree);
                    }
                    break;

                case KeyState.KeyThree:
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        print("youewin");
                        UsingPiano = false;
                        KeyBoard.enabled = false;
                        DoorCol.enabled = false;
                        DoorRend.enabled = false;
                    }
                    break;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                UsingPiano = true;
                KeyBoard.enabled = true;
            }
        }
        else
        {
            KeyBoard.enabled = false;
        }
    }
    public void PoorProgramming()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            KeyBoard.texture = animOne;
            ChangeState(KeyState.KeyOne);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            KeyBoard.texture = animTwo;
            ChangeState(KeyState.KeyOne);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            KeyBoard.texture = animFour;
            ChangeState(KeyState.KeyOne);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            KeyBoard.texture = animSix;
            ChangeState(KeyState.KeyOne);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            KeyBoard.texture = animSeven;
            ChangeState(KeyState.KeyOne);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            KeyBoard.texture = animThree;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            KeyBoard.texture = animFive;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            KeyBoard.texture = animEight;
        }

    }
}
