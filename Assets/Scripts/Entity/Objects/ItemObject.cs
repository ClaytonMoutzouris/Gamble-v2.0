using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Entity {

    public Item mItemData;

    public ItemObject(Item iData, EntityPrototype proto) : base(proto)
    {
        
        mItemData = iData;
        Body = new PhysicsBody(this, new CustomAABB(Position, new Vector2(5,5),new Vector2(0,5)));
        Body.mIsKinematic = false;

    }
    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
        Renderer.SetSprite(mItemData.sprite);

    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

    }

}
