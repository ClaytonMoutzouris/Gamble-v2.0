using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Entity {

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    public bool mOpen = false;


    public override void EntityInit()
    {
        //SetTilePosition(mMap.GetMapTileAtPoint(transform.position));
        //mPosition = RoundVector(transform.position);

        Body.mAABB.HalfSize = new Vector2(10.0f, 10.0f);
        //mAABB.Center = mPosition;
        Body.mIsKinematic = false;



        base.EntityInit();       


    }

    public override void EntityUpdate()
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
        temp.EntityInit();
        temp.Body.mPosition = Body.mPosition +new Vector2(0, MapManager.cTileSize/2);
        //temp.mOldSpeed.y = Constants.cJumpSpeed;
        //temp.mSpeed.y = Constants.cJumpSpeed*10;

        return true;
    }
}
