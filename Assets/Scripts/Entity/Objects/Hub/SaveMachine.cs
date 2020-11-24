using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMachine : Entity, IInteractable
{

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    private string interactLabel = "<Save>";


    public SaveMachine(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;


        Body.mIsKinematic = false;

    }

    public string InteractLabel { get => interactLabel; set => interactLabel = value; }

    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public bool Interact(Player actor)
    {
        SaveLoadManager.instance.SaveCharacter(actor);
        ShowFloatingText("*Saved*", Color.green);
        SoundManager.instance.PlaySingle(prototype.interactSFX[0]);
        return true;
    }
}