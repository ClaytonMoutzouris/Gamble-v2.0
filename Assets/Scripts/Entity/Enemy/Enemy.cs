using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Hostility { Friendly, Neutral, Hostile };

public abstract class Enemy : Entity, IHurtable
{
    public EnemyType mEnemyType;

    //Behaviour
    [SerializeField]
    private Hostility hostility = Hostility.Neutral;

    //End of Behaviour

    private Hurtbox hurtBox;
    private Sightbox sight;
    [HideInInspector]
    public AttackManager mAttackManager;

    [SerializeField]
    private Entity target = null;

    [HideInInspector]
    public Stats mStats;
    [HideInInspector]
    public EnemyBehaviour mBehaviour;

    public Hurtbox HurtBox
    {
        get
        {
            return hurtBox;
        }

        set
        {
            hurtBox = value;
        }
    }

    public Entity Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    public Hostility Hostility
    {
        get
        {
            return hostility;
        }

        set
        {
            hostility = value;
        }
    }

    

    public Sightbox Sight
    {
        get
        {
            return sight;
        }
        set
        {
            sight = value;
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (mStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Body.mAABB.Center + (Body.mAABB.HalfSizeY + 3) * Vector3.up, new Vector3(30 * (mStats.health.currentHealth / mStats.health.maxHealth), 6, 1));
        }

        if (sight != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(sight.mAABB.Center, sight.mAABB.HalfSize.x);
        }
    }

    public virtual void EnemyInit()
    {

        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, BodySize, Vector3.zero, new Vector3(1, 1, 1)));
        HurtBox.UpdatePosition();


        mStats = GetComponent<Stats>();
        sight = new Sightbox(this, new CustomAABB(transform.position, new Vector2(200, 200), Vector3.zero, new Vector3(1, 1, 1)));
        sight.UpdatePosition();

        mBehaviour = GetComponent<EnemyBehaviour>();
        mBehaviour.mEnemy = this;

        EnemyHealthBar temp = Instantiate(Resources.Load<EnemyHealthBar>("Prefabs/UI/EnemyHealthBar"), transform) as EnemyHealthBar;
        temp.transform.localPosition = new Vector3(0, BodySize.y * 2);
        temp.InitHealthbar(this);
        mStats.health.healthbar = temp;
        
        mStats.health.healthbar.SetHealth(mStats.health);

        mAttackManager = GetComponent<AttackManager>();

        MeleeAttack defaultAttack = new MeleeAttack(this, .5f, 5, .5f, new Hitbox(this, new CustomAABB(transform.position, Body.mAABB.HalfSize, new Vector3(Body.mAABB.HalfSizeX, 0), new Vector3(1, 1, 1))));
        mAttackManager.AttackList.Add(defaultAttack);
        mAttackManager.meleeAttacks.Add(defaultAttack);
    }

    public override void SecondUpdate()
    {
        //Should be in the habit of doing this first, i guess
        base.SecondUpdate();
        mAttackManager.SecondUpdate();

        HurtBox.UpdatePosition();
        sight.UpdatePosition();

    }

    /*1. Entities stroll around until their moveCooldown is reached.
    *2. If the entity hits a wall, they will wiggle until they turn around.
    *Consider fixing the wiggle.
    *3. If the strollTime reaches moveCooldown, the entity will stop moving.
    *4. The entity will consider where to move next when it is done waiting.
    *5. When it is done waiting strollTime is reset to 0f.
   */

 /*
    public void Move()
    {
        //1
        body.mSpeed.x = mMovingSpeed;
        //2
        if (strollTime < moveCooldown)
        {
            if (body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
            {
                mMovingSpeed *= -1;

            }
            strollTime += Time.deltaTime;
            return;
        }
        //3-4
        else if (strollTime > moveCooldown && strollTime < wait)
        {
            //Movement Cooldown reached .
            //reset strollTime.
            body.mSpeed.x = 0f;
            strollTime += Time.deltaTime;
            return;
        }
        //5
        else if (strollTime > wait)
        {
            strollTime = 0f;
            return;
        }
    }

    public void Move(Entity target, int direction)
    {
        //1-2
        if (strollTime < moveCooldown)
        {
            //If we have a target move in it's direction.
            if (target != null)
            {
                body.mSpeed.x = mMovingSpeed * direction;
            }

            strollTime += Time.deltaTime;
            return;
        }
        //3-4
        else if (strollTime > moveCooldown && strollTime < wait)
        {
            //Debug.Log("Resting");
            //Movement Cooldown reached .
            //reset strollTime.
            body.mSpeed.x = 0f;
            strollTime += Time.deltaTime;
            return;
        }
        //5
        else if (strollTime > wait)
        {
            //Debug.Log("Done Resting.");
            strollTime = 0f;
            return;
        }
    }

    public void Jump(Entity target, int direction)
    {

        //If we have a target, and we arent jumping.
        if (target != null && !jumping && jumpCooldown == 0)
        {
            this.Body.mSpeed.y = jumpSpeed * direction;
            jumping = true;
        }

        //If we have initiated a jump, add force to our mSpeed y.
        if (jumping && jumpCooldown <= jumpTime)
        {
            jumpCooldown += Time.deltaTime;
        }
        else if (jumping && jumpCooldown >= jumpTime)
        {
            jumpCooldown = 0;
            jumping = false;
        }
    }

    public void CheckForTargets()
    {
        //First enemies check sight
        target = null;
        if (sight.mEntitiesInSight != null)
        {
            foreach (Entity entity in sight.mEntitiesInSight)
            {
                if (entity is Player && Hostility == Hostility.Hostile)
                {
                    target = entity;
                    break;
                }
            }
        }
    }

    public void EnemyAttack()
    {
        if (target != null)
        {
            //If target is standing close to Entity
            if (Mathf.Abs(target.Position.x) - Mathf.Abs(this.Body.mPosition.x) < 20 && Mathf.Abs(target.Position.x) - Mathf.Abs(this.Body.mPosition.x) > -20 && Mathf.Abs(target.Position.y) - Mathf.Abs(this.Body.mPosition.y) < 30 && Mathf.Abs(target.Position.y) - Mathf.Abs(this.Body.mPosition.y) > -30)
            {
                //If target is to the left of the Entity && Target has an attack...
                if (target.Position.x < this.Body.mPosition.x && mAttackManager.AttackList != null)
                {
                    //Check if target can make a close range attack.
                    foreach (MeleeAttack attack in mAttackManager.AttackList)
                    {
                        if (!attack.mIsActive)
                        {
                            //Check if the hitbox has already set OffsetX to face to the left.
                            if (attack.hitbox.mAABB.OffsetX > 0)
                                attack.hitbox.mAABB.OffsetX = attack.hitbox.mAABB.OffsetX * -1;
                        }
                    }
                }
                //If target is to the right of the Entity && Target has an attack...
                else if (target.Position.x > this.Body.mPosition.x && mAttackManager.AttackList != null)
                {

                    //Check if target can make a close range attack.
                    foreach (MeleeAttack attack in mAttackManager.AttackList)
                    {
                        if (!attack.mIsActive)
                        {
                            attack.hitbox.mAABB.OffsetX = Mathf.Abs(attack.hitbox.mAABB.OffsetX);
                        }
                    }
                }
                //Attack
                mAttackManager.AttackList[0].Activate();
            }
        }

        mAttackManager.UpdateAttacks();
    }
    */

    public void EnemyUpdate()
    {
        mBehaviour.EnemyBehaviourUpdate(mBehaviour.mEnemy);

        /*
         * 
        //Enemy checks Sightbox for targets.
        mBehaviour.CheckForTargets(this);

        if (target != null)
        {
            if (target.Position.x > this.Body.mPosition.x)
            {
                Move(target, 1);
                if (target.Position.y - this.Body.mPosition.y > 30)
                {
                    Jump(target, 1);
                }

            }
            else if (target.Position.x < this.Body.mPosition.x)
            {
                //Debug.Log("Target: " + target.Position.y + "Slime: " + this.Body.mPosition.y);
                Move(target, -1);
                if (target.Position.y - this.Body.mPosition.y > 30)
                {
                    Debug.Log("jumping!");
                    Jump(target, 1);
                }
            }
        }
        else
        {
            Move();
        }

        //Enemy attack a target if it is hostile towards it.
        EnemyAttack();
        */
    }



    public override void Die()
    {
        base.Die();
        DropLoot();
        HurtBox.mState = ColliderState.Closed;
    }

    public virtual void DropLoot()
    {
        ItemObject temp = Instantiate(Resources.Load<ItemObject>("Prefabs/ItemObject")) as ItemObject;
        temp.SetItem(ItemDatabase.GetRandomItem());
        temp.EntityInit();
        temp.Body.mPosition = Body.mPosition + new Vector2(0, MapManager.cTileSize / 2);
    }

    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(HurtBox);
        CollisionManager.RemoveObjectFromAreas(sight);


        base.ActuallyDie();
    }

    public virtual void GetHurt(Attack attack)
    {
        Debug.Log("Dude is getting hurt");
        if (Hostility == Hostility.Neutral)
        {
            Hostility = Hostility.Hostile;
        }

        int damage = (int)mStats.health.LoseHP(attack.damage);
        ShowFloatingText(damage, Color.white);

        Debug.Log("Current health: " + mStats.health.currentHealth + " damage");

        if (mStats.health.currentHealth == 0)
        {
            Die();
        }
      
    }


}