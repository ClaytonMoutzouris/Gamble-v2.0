using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Maybe subclass of enemy instead?
public class EnemyPart : Enemy
{
    //The entity this is a subpart of
    public Enemy parentEntity;
    public Vector2 offset;

    public EnemyPart(EnemyPrototype proto, Enemy parent) : base(proto)
    {
        mEnemyType = EnemyType.SubEnemy;
        parentEntity = parent;

        

    }

    public override void GetHurt(Attack attack)
    {
        parentEntity.GetHurt(attack);
    }

}
