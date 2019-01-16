using UnityEngine;
using System.Collections;

//consider nuking this

public class Constants
{
    public const float cGravity = -1030.0f;
    public const float cMaxFallingSpeed = -900.0f;
    public const float cConveyorSpeed = 100.0f;
    public const float cBounceSpeed = 700.0f;
    public const float cHalfSizeY = 12.0f;
    public const float cHalfSizeX = 6.0f;
    public const float cMinJumpSpeed = 200.0f;
    public const float cOneWayPlatformThreshold = 2.0f;
    //This allows to prevent crushing if the moving object is within a margin of error in the other axis
    public const float cCrushCorrectThreshold = 3.0f;
    //public const float cJumpSpeed = 210.0f; //1
    //public const float cJumpSpeed = 280.0f; //2
    //public const float cJumpSpeed = 350.0f; //3
    //public const float cJumpSpeed = 380.0f; //4
    //public const float cJumpSpeed = 410.0f; //5
    //public const float cJumpSpeed = 460.0f; //6
    public const int cDefaultMapWidth = 100;
    public const int cDefaultMapHeight = 100;
    public const int cMapChunkSizeX = 10;
    public const int cMapChunkSizeY = 10;
    public const int cTileResolution = 32;



    public static readonly float[] cJumpSpeeds = { 210.0f, 280.0f, 350.0f, 380.0f, 410.0f, 460.0f };
    public static readonly float[] cHalfSizes = { 6.0f, 12.0f, 20.0f, 30.0f, 36.0f, 42.0f, 50.0f, 60.0f, 62.0f};
    //Still needs tweaking
    public const int cJumpFramesThreshold = 10;

    public const float cBotMaxPositionError = 1.0f;

	public const float cGrabLedgeStartY = 0.0f;
    public const float cGrabLedgeEndY = 2.0f;
    public const float cGrabLedgeTileOffsetY = -4.0f;

    public const int cSlopeWallHeight = 4;
}
