using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack {

    public Entity mEntity;

    public int damage;
    public float duration;
    public float elapsed;

    public bool mIsActive = false;

    public Hitbox hitbox;

    public Attack()
    {

    }

    public void UpdateAttack()
    {
        if(elapsed < duration)
        {
            elapsed += Time.deltaTime;
        }

        if(elapsed >= duration)
        {
            Deactivate();
        }
    }
    
    public void Activate()
    {
        if (mIsActive)
            return;

        elapsed = 0;
        mIsActive = true;
        hitbox.state = ColliderState.Open;
    }

    public void Deactivate()
    {
        elapsed = 0;
        mIsActive = false;
        hitbox.state = ColliderState.Closed;
    }

    public void Hit(Hurtbox hurtbox)
    {
        hurtbox.GetHit(this);
    }
	
}
