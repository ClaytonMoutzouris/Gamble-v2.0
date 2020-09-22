using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : Ability
{
    Sightbox sightbox;
    List<Entity> effectedEntities = new List<Entity>();
    public int auraRadius = 160;
    public Ability auraAbility;
    public ParticleSystem visuals;

    public void Apply(Entity entity)
    {
        if(entity is Player player)
        {
            auraAbility.OnEquipTrigger(player);
            player.Renderer.AddVisualEffect(visuals, Vector2.up*player.Body.mAABB.HalfSize);
        }
    }

    public void Unapply(Entity entity)
    {
        auraAbility.OnUnequipTrigger(entity);
        entity.Renderer.RemoveVisualEffect(abilityName+"(Clone)");
        
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
        sightbox = new Sightbox(owner, new CustomAABB(owner.Position, Vector2.one * auraRadius, Vector2.zero));
        sightbox.UpdatePosition();
        Apply(player);

    }

    public override void OnUnequipTrigger(Entity entity)
    {
        Unapply(entity);

        base.OnUnequipTrigger(entity);

        if (sightbox != null)
        {
            CollisionManager.RemoveObjectFromAreas(sightbox);
        }
        


        foreach (Entity effected in effectedEntities)
        {
            Unapply(effected);
        }
    }

    public override void OnUpdate() {
        base.OnUpdate();

        List<Entity> entitiesToRemove = new List<Entity>();

        foreach(Entity entity in effectedEntities)
        {
            if(!sightbox.mEntitiesInSight.Contains(entity))
            {
                entitiesToRemove.Add(entity);
                Debug.LogWarning(entity.entityName + " no longer in range");

            }
        }

        foreach(Entity entity in entitiesToRemove)
        {
            //unapply aura
            Unapply(entity);
            effectedEntities.Remove(entity);
        }

        foreach(Entity entity in sightbox.mEntitiesInSight)
        {
            if(entity is Player player && !effectedEntities.Contains(player))
            {
                //apply aura
                Apply(player);

                effectedEntities.Add(player);
            }
        }


        CollisionManager.UpdateAreas(sightbox);
        sightbox.mEntitiesInSight.Clear();


    }

    public override void OnSecondUpdate()
    {
        base.OnSecondUpdate();
        sightbox.UpdatePosition();
    }

}

