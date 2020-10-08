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
    public GameObject auraObject;

    public void Apply(Entity entity)
    {
        if(entity is Player player)
        {
            auraAbility.OnEquipTrigger(player);
            auraObject = player.Renderer.AddVisualEffect(visuals, Vector2.up*player.Body.mAABB.HalfSize);
        }

        if (entity is Companion companion)
        {
            Debug.LogWarning("Applying auro to companion");

            auraAbility.OnEquipTrigger(companion);
            auraObject = companion.Renderer.AddVisualEffect(visuals, Vector2.up * companion.Body.mAABB.HalfSize);
        }
    }

    public void Unapply(Entity entity)
    {
        auraAbility.OnUnequipTrigger(entity);
        entity.Renderer.RemoveVisualEffect(auraObject);
        
    }

    public override void OnEquipTrigger(Entity player)
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

            if (entity is Companion companion && !effectedEntities.Contains(companion))
            {
                //apply aura
                Apply(companion);
                Debug.LogWarning("There is a companion in range");

                effectedEntities.Add(companion);
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

