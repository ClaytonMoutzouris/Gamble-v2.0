using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaGadget : Gadget
{

    public RangedAttackPrototype attackProto;

    public override bool Activate(Player player, int index)
    {
        if(!base.Activate(player, index)) {
            return false;
        }


        float interval = attackProto.spreadAngle / attackProto.numberOfProjectiles;

        for (int i = 0; i < attackProto.numberOfProjectiles; i++)
        {

            Vector2 tempDir = Vector2.right*(int)player.mDirection;

            if (attackProto.numberOfProjectiles > 1)
            {
                tempDir = tempDir.Rotate(-attackProto.spreadAngle / 2 + (interval * i));
            }

            tempDir.Normalize();

            Projectile shot = new Projectile(attackProto.projectilePrototype, new RangedAttack(player, attackProto), tempDir);
            shot.Spawn(player.Position + new Vector2(0, attackProto.offset.y) + (attackProto.offset * tempDir.normalized));
        }

        return true;
    }

}
