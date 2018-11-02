using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    
    public Character mPlayer;
    bool[] mInputs;
    bool[] mPrevInputs;

    public Transform mCharacterPrefab;

    public Map mMap;


    public List<MovingObject> mObjects = new List<MovingObject>();


    void Start()
    {
        mInputs = new bool[(int)KeyInput.Count];
        mPrevInputs = new bool[(int)KeyInput.Count];

        mPlayer.mGame = this;
        mPlayer.mMap = mMap;
        mPlayer.CharacterInit(mInputs, mPrevInputs);
        mPlayer.mType = ObjectType.Player;

        mObjects[1].mGame = this;
        mObjects[1].mMap = mMap;
        ((Character)mObjects[1]).CharacterInit(new bool[(int)KeyInput.Count], new bool[(int)KeyInput.Count]);
        mObjects[1].mType = ObjectType.NPC;
        mMap.InitMap();
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
        for (int i = 0; i < mObjects.Count; ++i)
        {
            switch (mObjects[i].mType)
            {
                case ObjectType.Player:
                    
                    
                    
                case ObjectType.NPC:
                    ((Character)mObjects[i]).CharacterUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
            }
        }

        mMap.CheckCollisions();
        for (int i = 0; i < mObjects.Count; ++i)
            mObjects[i].UpdatePhysicsPartTwo();
    }
}