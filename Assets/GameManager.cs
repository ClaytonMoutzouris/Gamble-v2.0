using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    
    public Character mPlayer;
    bool[] mInputs;
    bool[] mPrevInputs;

    void Start()
    {
        mInputs = new bool[(int)KeyInput.Count];
        mPrevInputs = new bool[(int)KeyInput.Count];

        mPlayer.CharacterInit(mInputs, mPrevInputs);
    }

    void Update()
    {
        mInputs[(int)KeyInput.GoRight] = Input.GetKey(KeyCode.RightArrow);
        mInputs[(int)KeyInput.GoLeft] = Input.GetKey(KeyCode.LeftArrow);
        mInputs[(int)KeyInput.GoDown] = Input.GetKey(KeyCode.DownArrow);
        mInputs[(int)KeyInput.Jump] = Input.GetKey(KeyCode.Space);
    }

    void FixedUpdate()
    {
        mPlayer.CharacterUpdate();
    }
}