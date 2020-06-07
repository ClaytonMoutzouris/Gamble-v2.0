using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : Effect
{
    Sightbox sightbox;
    float tickFrequency = 32;
    public List<Entity> effectedEntities;
    public int auraRadius = 160;
    StatBonus bonus;
    ParticleSystem visuals;

    public Aura()
    {
        effectName = "Aura";
        type = EffectType.Aura;
        //currentPosition = 
        bonus = new StatBonus(StatType.Speed, 10);
        effectedEntities = new List<Entity>();
        visuals = Resources.Load<ParticleSystem>("ParticleEffects/Aura");
    }

    public void Apply(Entity entity)
    {
        if(entity is Player player)
        {
            player.mStats.AddBonus(bonus);
            player.Renderer.AddVisualEffect(visuals, Vector2.up*player.Body.mAABB.HalfSize);
        }
    }

    public void Unapply(Entity entity)
    {
        if (entity is Player player)
        {
            player.mStats.RemoveBonus(bonus);
            player.Renderer.RemoveVisualEffect("Aura(Clone)");
        }
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
        sightbox = new Sightbox(owner, new CustomAABB(owner.Position, Vector2.one * auraRadius, Vector2.zero));
        sightbox.UpdatePosition();
        Apply(player);

    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        CollisionManager.RemoveObjectFromAreas(sightbox);
        Unapply(player);

    }

    public override void OnPlayerDeath(Player player)
    {
        Unapply(player);

        base.OnPlayerDeath(player);
        CollisionManager.RemoveObjectFromAreas(sightbox);

        foreach (Entity entity in effectedEntities)
        {
            Unapply(entity);
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

