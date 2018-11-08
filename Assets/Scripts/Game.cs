using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{

    public Camera gameCamera;
    public Character player;
    bool[] inputs;
    bool[] prevInputs;

    public KeyCode goLeftKey = KeyCode.A;
    public KeyCode goRightKey = KeyCode.D;
    public KeyCode goJumpKey = KeyCode.W;
    public KeyCode goDownKey = KeyCode.S;

    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    public Map mMap;

    public TileType mPlacedTileType;

    public Transform mCharacterPrefab;
    public Transform mMovingPlatformPrefab;
    public Transform mSlimePrefab;
    [SerializeField]
    protected List<MovingObject> mObjects = new List<MovingObject>();

    public SpriteRenderer mMouseSprite;

    void Start ()
    {
        Application.targetFrameRate = 60;

        inputs = new bool[(int)KeyInput.Count];
        prevInputs = new bool[(int)KeyInput.Count];

        player.mGame = this;
        player.mMap = mMap;
        player.Init(inputs, prevInputs);
        player.mType = ObjectType.Player;
       // player.Scale = Vector2.one * 1.5f;

        mMap.Init();
    }
	
	void Update()
    {
        inputs[(int)KeyInput.GoRight] = Input.GetKey(KeyCode.RightArrow);
        inputs[(int)KeyInput.GoLeft] = Input.GetKey(KeyCode.LeftArrow);
        inputs[(int)KeyInput.GoDown] = Input.GetKey(KeyCode.DownArrow);
        inputs[(int)KeyInput.Jump] = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.D))
        {
            mMap.DebugPrintAreas();
        }

        //UpdateCursor();

    }
    /*
    public void UpdateCursor()
    {
        if (Input.GetKeyUp(KeyCode.Mouse2))
            lastMouseTileX = lastMouseTileY = -1;

        Vector2 mousePos = Input.mousePosition;
        Vector2 cameraPos = Camera.main.transform.position;
        var mousePosInWorld = cameraPos + mousePos - new Vector2(gameCamera.pixelWidth / 2, gameCamera.pixelHeight / 2);

        int mouseTileX, mouseTileY;

        mMap.GetMapTileAtPoint(mousePosInWorld, out mouseTileX, out mouseTileY);
        //Debug.Log("Pressed X: " + mouseTileX + ", Y: " + mouseTileY);

        //Vector2 offsetMouse = (Vector2)(Input.mousePosition) - new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

        if (Input.GetKeyDown(KeyCode.Tab))
            Debug.Break();

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0))
        {
            if (mouseTileX != lastMouseTileX || mouseTileY != lastMouseTileY || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2))
            {
                if (!mMap.IsNotEmpty(mouseTileX, mouseTileY))
                    mMap.SetTile(mouseTileX, mouseTileY, mPlacedTileType);
                else
                    mMap.SetTile(mouseTileX, mouseTileY, TileType.Empty);

                lastMouseTileX = mouseTileX;
                lastMouseTileY = mouseTileY;

                Debug.Log(mouseTileX + "  " + mouseTileY);
            }
        }


            var wheel = Input.GetAxis("Mouse ScrollWheel");
            if (wheel > 0.05f || Input.GetKeyUp(KeyCode.RightArrow))
            {

                if ((int)mPlacedTileType < (int)TileType.Count - 1)
                    mPlacedTileType += 1;
                else
                    mPlacedTileType = TileType.Block;


            }
            else if (wheel < -0.05f || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if ((int)mPlacedTileType > (int)TileType.Empty + 1)
                    mPlacedTileType -= 1;
                else
                    mPlacedTileType = TileType.Count - 1;

            }
        

        //mMouseSprite.transform.position = mMap.GetMapTilePosition(mouseTileX, mouseTileY);
    }
    */
    public void SwapUpdateIds(MovingObject a, MovingObject b)
    {
        int tmp = a.mUpdateId;
        a.mUpdateId = b.mUpdateId;
        b.mUpdateId = tmp;
        mObjects[a.mUpdateId] = a;
        mObjects[b.mUpdateId] = b;
    }

    public int AddToUpdateList(MovingObject obj)
    {
        mObjects.Add(obj);
        return mObjects.Count - 1;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < mObjects.Count; ++i)
        {
            switch (mObjects[i].mType)
            {
                case ObjectType.Player:
                
                case ObjectType.NPC:
                    ((Character)mObjects[i]).CustomUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
                case ObjectType.Enemy:
                    ((Slime)mObjects[i]).CustomUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
                case ObjectType.MovingPlatform:
                    ((MovingPlatform)mObjects[i]).CustomUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
            }
        }

        mMap.CheckCollisions();

        for (int i = 0; i < mObjects.Count; ++i)
            mObjects[i].UpdatePhysicsP2();
    }
}
