using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponAbilities
{

    public static void Explode(Entity owner, Vector2 position)
    {
        //TODO: spawn an explosion here
        Debug.Log("BOOM");
        //ProjectilePrototype prototype = Resources.Load<ProjectilePrototype>("Prototypes/Entity/Projectile/Explosion") as ProjectilePrototype;
        //RangedAttack rattack = new RangedAttack(owner, 0.5f, 50, 0, 1, 0, prototype, Vector2.zero, new List<WeaponAbility>());
        //Projectile shot = new Projectile(prototype, rattack, Vector2.zero);
        //shot.Spawn(position);
    }

    public static void Split(Projectile original, IHurtable hit)
    {
        
        /*
        RangedAttack rattack = new RangedAttack(original.owner, 0.5f, 50, 0, 1, 0, original.projProto, Vector2.zero, new List<WeaponAbility>());
        Projectile shot = new Projectile(original.projProto, rattack, original.direction + new Vector2(-0.5f, 0.5f));
        shot.hitbox.mDealtWith.Add(hit);
        shot.Spawn(original.Position);

        RangedAttack rattack2 = new RangedAttack(original.owner, 0.5f, 50, 0, 1, 0, original.projProto, Vector2.zero, new List<WeaponAbility>());
        Projectile shot2 = new Projectile(original.projProto, rattack2, original.direction + new Vector2(0.5f, 0.5f));
        shot2.hitbox.mDealtWith.Add(hit);
        shot2.Spawn(original.Position);
        */
    }

}
