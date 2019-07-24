using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



[System.Serializable]
public struct PositionState
{

    public bool pushesRight;
    public bool pushesLeft;
    public bool pushesBottom;
    public bool pushesTop;

    public bool pushedTop;
    public bool pushedBottom;
    public bool pushedRight;
    public bool pushedLeft;

    public bool pushedLeftObject;
    public bool pushedRightObject;
    public bool pushedBottomObject;
    public bool pushedTopObject;

    public bool pushesLeftObject;
    public bool pushesRightObject;
    public bool pushesBottomObject;
    public bool pushesTopObject;

    public bool pushedLeftTile;
    public bool pushedRightTile;
    public bool pushedBottomTile;
    public bool pushedTopTile;

    public bool pushesLeftTile;
    public bool pushesRightTile;
    public bool pushesBottomTile;
    public bool pushesTopTile;

    public bool onOneWay;
    public bool tmpIgnoresOneWay;
    public bool tmpSticksToSlope;
    public int oneWayY;
    public bool onLadder;
    public bool isClimbing;
    public bool isBounce;
    public bool onDoor;
    public bool isJetting;

    public Vector2i leftTile;
    public Vector2i rightTile;
    public Vector2i topTile;
    public Vector2i bottomTile;

    public void Reset()
    {
        leftTile = rightTile = topTile = bottomTile = new Vector2i(-1, -1);
        oneWayY = -1;

        pushesRight = false;
        pushesLeft = false;
        pushesBottom = false;
        pushesTop = false;

        pushedTop = false;
        pushedBottom = false;
        pushedRight = false;
        pushedLeft = false;

        pushedLeftObject = false;
        pushedRightObject = false;
        pushedBottomObject = false;
        pushedTopObject = false;

        pushesLeftObject = false;
        pushesRightObject = false;
        pushesBottomObject = false;
        pushesTopObject = false;

        pushedLeftTile = false;
        pushedRightTile = false;
        pushedBottomTile = false;
        pushedTopTile = false;

        pushesLeftTile = false;
        pushesRightTile = false;
        pushesBottomTile = false;
        pushesTopTile = false;

        onOneWay = false;
        onLadder = false;
        onDoor = false;
        isClimbing = false;
        isBounce = false;

    }
}