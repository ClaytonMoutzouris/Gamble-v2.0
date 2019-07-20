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


    public Chest() : base()
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, new Vector2(8,8), Vector2.zero));

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
        Renderer.SetAnimState("ChestOpen");

        ItemObject temp = new ItemObject(ItemDatabase.GetRandomItem());
        temp.Spawn(Position + new Vector2(0, MapManager.cTileSize / 2));
        //temp.Position = Body.mPosition +new Vector2(0, MapManager.cTileSize/2);
        //temp.mOldSpeed.y = Constants.cJumpSpeed;
        //temp.mSpeed.y = Constants.cJumpSpeed*10;

        return true;
    }
}
