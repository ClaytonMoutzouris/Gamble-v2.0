using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EntityRenderer : MonoBehaviour
{
    SpriteRenderer sprite;
    Animator animator;

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
        colorSwapper = new ColorSwap(sprite);
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

        if(Entity is Player)
        {
            Player P = (Player)Entity;
            //Debug.Log("Enemy Hurtbox Center " + E.HurtBox.mAABB.Center + " and Size: " + E.HurtBox.mAABB.HalfSize);

            Gizmos.DrawCube(P.HurtBox.mAABB.Center, P.HurtBox.mAABB.HalfSize * 2);

            if(P.AttackManager != null && P.AttackManager.meleeAttacks != null && P.AttackManager.meleeAttacks[0] != null && P.AttackManager.meleeAttacks[0].mIsActive)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawCube(P.AttackManager.meleeAttacks[0].hitbox.mAABB.Center, P.AttackManager.meleeAttacks[0].hitbox.mAABB.HalfSize* 2);

                
            }
        }

        

        Gizmos.color = Color.green;

        //Gizmos.DrawWireCube(Entity.Body.mAABB.Center, Entity.Body.mAABB.HalfSize*2);



    }

    public void SetEntity(Entity entity)
    {
        this.entity = entity;
        
    }

    public void Draw()
    {
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

}
