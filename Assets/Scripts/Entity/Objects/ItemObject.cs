using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Entity, ILootable {

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
        Debug.Log("Renderer: " + Renderer == null);
        Debug.Log("mItemData: " + mItemData == null);
        Debug.Log("sprite: " + mItemData.sprite == null);

        Renderer.SetSprite(mItemData.sprite);
        //temp.mOldSpeed.y = Constants.cJumpSpeed;
        Body.mSpeed.y = Constants.cMinJumpSpeed;
        Body.mSpeed.x = Random.Range(-40, 40);
    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

        if(Body.mPS.pushesBottom)
        {
            Body.mSpeed.x = 0;
        }


    }

    public bool Loot(Player actor)
    {

        return actor.PickUp(this);
    }
}
