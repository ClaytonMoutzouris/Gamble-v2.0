using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Entity {

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    [HideInInspector]
    public bool mOpen = false;


    public override void EntityInit()
    {
        base.EntityInit();

        //SetTilePosition(mMap.GetMapTileAtPoint(transform.position));
        //mPosition = RoundVector(transform.position);

        //mAABB.Center = mPosition;
        Body.mIsKinematic = false;





    }

    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public bool OpenChest()
    {
        if (mOpen)
            return false;

        mOpen = true;
        mAnimator.Play("ChestOpen");
        
        ItemObject temp = Instantiate(Resources.Load<ItemObject>("Prefabs/ItemObject")) as ItemObject;
        temp.SetItem(ItemDatabase.GetRandomItem());
        temp.EntityInit();
        temp.Body.mPosition = Body.mPosition +new Vector2(0, MapManager.cTileSize/2);
        //temp.mOldSpeed.y = Constants.cJumpSpeed;
        //temp.mSpeed.y = Constants.cJumpSpeed*10;

        return true;
    }
}
