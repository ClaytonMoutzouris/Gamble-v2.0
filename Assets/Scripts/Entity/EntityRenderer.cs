using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EntityRenderer : MonoBehaviour
{
    SpriteRenderer sprite;
    Animator animator;
    List<GameObject> effects = new List<GameObject>();

    private Entity entity;

    public SpriteRenderer Sprite
    {
        get
        {
            return sprite;
        }

        set
        {
            sprite = value;
        }
    }

    public Animator Animator
    {
        get
        {
            return animator;
        }

        set
        {
            animator = value;
        }
    }

    public Entity Entity
    {
        get
        {
            return entity;
        }

        set
        {
            entity = value;
        }
    }

    public ColorSwap colorSwapper;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        colorSwapper = new ColorSwap(sprite.material);
        //AddVisualEffect();
    }

    public void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        //Debug.Log("Enemy Hurtbox Center " + Entity.Body.mAABB.HalfSize);
        //Gizmos.DrawWireSphere(Entity.Body.mAABB.Center, Entity.Body.mAABB.HalfSize.x);
        if(Entity is Enemy)
        {
            Enemy E = (Enemy)Entity;
            //Debug.Log("Enemy Hurtbox Center " + E.HurtBox.mAABB.Center + " and Size: " + E.HurtBox.mAABB.HalfSize);

            Gizmos.DrawCube(E.HurtBox.mAABB.Center, E.HurtBox.mAABB.HalfSize * 2);

        }

        if (Entity is Player)
        {
            Player P = (Player)Entity;
            //Debug.Log("Enemy Hurtbox Center " + E.HurtBox.mAABB.Center + " and Size: " + E.HurtBox.mAABB.HalfSize);

            Gizmos.DrawCube(P.HurtBox.mAABB.Center, P.HurtBox.mAABB.HalfSize * 2);




            if (P.AttackManager != null && P.AttackManager.meleeAttacks != null && P.AttackManager.meleeAttacks[0] != null && P.AttackManager.meleeAttacks[0].mIsActive)
            {
                Gizmos.color = Color.blue;
                
            }

        }

        if (Entity is AttackObject)
        {
            if(Entity is MeleeAttackObject)
            {
                MeleeAttackObject mAttack = (MeleeAttackObject)Entity;
                if (mAttack.hitbox.mState == ColliderState.Open)
                {
                    Gizmos.color = Color.blue;

                    AttackObject E = (AttackObject)Entity;
                    //Debug.Log("Enemy Hurtbox Center " + E.HurtBox.mAABB.Center + " and Size: " + E.HurtBox.mAABB.HalfSize);

                    Gizmos.DrawCube(E.hitbox.mAABB.Center, E.hitbox.mAABB.HalfSize * 2);
                }
            }
            else
            {
                Gizmos.color = Color.blue;

                AttackObject E = (AttackObject)Entity;
                //Debug.Log("Enemy Hurtbox Center " + E.HurtBox.mAABB.Center + " and Size: " + E.HurtBox.mAABB.HalfSize);

                Gizmos.DrawCube(E.hitbox.mAABB.Center, E.hitbox.mAABB.HalfSize * 2);
            }


        }

        if(Entity is ForceField forcefield)
        {
            if (forcefield.shieldBox.mState == ColliderState.Open)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawCube(forcefield.shieldBox.mAABB.Center, forcefield.shieldBox.mAABB.HalfSize * 2);
            }
        }

        Gizmos.color = Color.green;

        //Gizmos.DrawWireCube(Entity.Body.mAABB.Center, Entity.Body.mAABB.HalfSize*2);



    }

    public void SetEntity(Entity entity)
    {
        this.entity = entity;
        
    }

    public virtual void Draw()
    {
        /*
        if(Entity.gravityVector == Vector2.up)
        {
            sprite.flipY = true;
        } else
        {
            sprite.flipY = false;
        }
        */
        sprite.flipX = (Entity.mDirection == EntityDirection.Left);
        transform.position = entity.Position;
        
    }

    public void SetSprite(Sprite sprite)
    {
        Sprite.sprite = sprite;
    }

    public void SetAnimState(string state)
    {
        animator.Play(state);
    }


    public GameObject AddVisualEffect(ParticleSystem particleEffect, Vector2 offset)
    {
        //gameObject.AddComponent<ParticleSystem>().;
        GameObject effectObject = Instantiate(particleEffect, transform).gameObject;
        effectObject.transform.localPosition = Entity.Body.mOffset + offset;
        effects.Add(effectObject);
        return effectObject;
    }

    public void RemoveVisualEffect(GameObject effect)
    {
        //effects.Remove(instan)
        effects.Remove(effect);
        Destroy(effect);
    }

    public void RemoveVisualEffect(string effect)
    {
        GameObject toRemove = new GameObject();
        //effects.Remove(instan)
        foreach(GameObject obj in effects)
        {
            if(obj && obj.name.Equals(effect))
            {
                toRemove = obj;
                break;
            }
        }

        effects.Remove(toRemove);
        Destroy(toRemove);

    }

    public void ClearVisualEffects()
    {
        foreach(GameObject effect in effects)
        {
            Destroy(effect);
        }
    }
}
