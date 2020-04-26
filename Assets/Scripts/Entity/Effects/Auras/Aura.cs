using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : Effect
{
    Sightbox sightbox;
    float tickFrequency = 32;
    public List<Entity> effectedEntities;
    public int auraRange = 320;

    public Aura()
    {
        effectName = "Aura";
        type = EffectType.Aura;
        //currentPosition = 

    }


    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
        sightbox = new Sightbox(owner, new CustomAABB(owner.Position, Vector2.one * auraRange / 2, Vector2.zero));
        sightbox.UpdatePosition();
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        CollisionManager.RemoveObjectFromAreas(sightbox);
    }
    public override void OnUpdate() {
        base.OnUpdate();

        List<Entity> previousEffected = effectedEntities;
        
        foreach(Entity entity in previousEffected)
        {
            if(!sightbox.mEntitiesInSight.Contains(entity))
            {
                //unapply aura
                effectedEntities.Remove(entity);
            }
        }

        foreach(Entity entity in sightbox.mEntitiesInSight)
        {
            if(effectedEntities.Contains(entity))
            {

            }
        }


        CollisionManager.UpdateAreas(sightbox);


    }

    public override void OnSecondUpdate()
    {
        base.OnSecondUpdate();
        sightbox.UpdatePosition();
    }

}

