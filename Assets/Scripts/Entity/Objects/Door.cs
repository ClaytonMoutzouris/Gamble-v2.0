using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Entity, IInteractable
{

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    [HideInInspector]
    public bool locked = false;
    public bool isVisible = true;
    List<Sprite> spritesForWorld;
    private string interactLabel = "<Enter>";

    public string InteractLabel { get => interactLabel; set => interactLabel = value; }

    public Door(DoorPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, Vector2.zero));

        Body.mIsKinematic = false;
        spritesForWorld = proto.spritesForWorlds;
    }


    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public void SetVisiblity(bool visible)
    {
        isVisible = visible;

        if(isVisible)
        {
            
        }
    }

    public bool Interact(Player actor)
    {
        if(locked)
        {
            InventorySlot temp = actor.Inventory.FindKeySlot();
            if (temp != null)
            {
                temp.GetOneItem();
                locked = false;
                ShowFloatingText("Unlocked!", Color.green);
                SoundManager.instance.PlaySingle(prototype.interactSFX[0]);
                return true;
            } else
            {
                ShowFloatingText("Locked", Color.red);
                return false;
            }
        } else
        {
            GameManager.instance.TravelToNextMap();
        }

        return true;
    }
    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        Renderer.SetSprite(spritesForWorld[(int)MapManager.instance.mCurrentMap.worldType]);
        //Change the door sprite accordingly
    }
    

    
}
