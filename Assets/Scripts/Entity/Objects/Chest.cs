using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : PhysicsObject {

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    public Animator mAnimator;
    public bool mOpen = false;
    public void Start()
    {
        if (mUpdateId < 0)
        {
            ObjectInit();
            //mSpeed.x = 0;
        }
    }

    public override void ObjectInit()
    {
        //SetTilePosition(mMap.GetMapTileAtPoint(transform.position));
        //mPosition = RoundVector(transform.position);

        mAABB.HalfSize = new Vector2(10.0f, 10.0f);
        //mAABB.Center = mPosition;
        mIsKinematic = false;



        base.ObjectInit();       


    }

    public override void CustomUpdate()
    {
        
        //UpdatePhysics();

    }

    public bool OpenChest()
    {
        if (mOpen)
            return false;

        mOpen = true;
        mAnimator.Play("ChestOpen");
        
        ItemObject temp = Instantiate(ItemDatabase.GetRandomItem());
        temp.ObjectInit();
        temp.mPosition = mPosition+new Vector2(0, MapManager.cTileSize/2);
        //temp.mOldSpeed.y = Constants.cJumpSpeed;
        //temp.mSpeed.y = Constants.cJumpSpeed*10;

        return true;
    }
}
