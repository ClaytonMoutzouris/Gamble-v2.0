using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Entity, ILootable, IContactTrigger {

    public Item mItemData;
    public bool showName = false;

    public ItemObject(Item iData, EntityPrototype proto) : base(proto)
    {

        mItemData = iData;

        Body = new PhysicsBody(this, new CustomAABB(Position, new Vector2(5,5),new Vector2(0,5)));
        Body.mIsKinematic = false;

    }
    public override void Spawn(Vector2 spawnPoint)
    {
        GameObject gameObject = GameObject.Instantiate(Resources.Load("Prefabs/ItemObjectRenderer")) as GameObject;
        Renderer = gameObject.GetComponent<ItemObjectRenderer>();
        Renderer.SetEntity(this);
        ((ItemObjectRenderer)Renderer).SetItem(mItemData);

        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }

        Position = spawnPoint + Body.mOffset;
        Renderer.Sprite.sortingLayerName = prototype.sortingLayer.ToString();
        Renderer.Draw();
        if (prototype.particleEffects != null)
        {
            Renderer.AddVisualEffect(prototype.particleEffects, prototype.bodySize * Vector2.up);
        }

        Body.UpdatePosition();
        isSpawned = true;

        Renderer.SetSprite(mItemData.sprite);
        //temp.mOldSpeed.y = Constants.cJumpSpeed;
        Body.mSpeed.y = Constants.cMinJumpSpeed;
        Body.mSpeed.x = Random.Range(-40, 40);
    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

        if(showName)
        {
            ((ItemObjectRenderer)Renderer).ShowText();
        } else
        {
            ((ItemObjectRenderer)Renderer).HideText();

        }

        showName = false;
        if(Body.mPS.pushesBottom)
        {
            Body.mSpeed.x = 0;
        }


    }

    public bool Loot(Player actor)
    {

        return actor.PickUp(this);
    }

    public void Contact(Entity entity)
    {
        if(!(entity is Player))
        {
            return;
        }
        showName = true;
    }
}
