using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyObject : PhysicsObject
{

    public float mMovingSpeed;
    public AABB mHitbox;
    public EnemyType mEnemyType;

    public void Start()
    {
        if (mUpdateId < 0)
        {
            ObjectInit();
            //mSpeed.x = 0;
        }
    }

    
}
