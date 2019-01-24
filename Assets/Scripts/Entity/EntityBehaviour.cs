using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{

    public Enemy mEnemy;
    private State state = State.Idle;
    
    //When the moveTimer reaches moveDuration they stop.
    public float moveDuration;
    //Timer for movement, they move for this amount of time.
    public float moveTimer;
    //Makes the character wait for an amount of time.
    public float wait;
    public float jumpDuration;
    public float jumpTimer;
    public bool jumping;
    public float jumpSpeed;
    public bool canJump;

    public EntityBehaviour(Enemy enemy)
    {
        mEnemy = enemy;
    }

    void EntityBehaviourInit()
    {
        canJump = false;
    }

    public void EntityBehaviourUpdate(Enemy enemy)
    {
        EnemyBehaviourLoop(enemy);
    }

    /*1. Entities stroll around until their moveCooldown is reached.
    *2. If the entity hits a wall, they will wiggle until they turn around.
    *Consider fixing the wiggle.
    *3. If the strollTime reaches moveCooldown, the entity will stop moving.
    *4. The entity will consider where to move next when it is done waiting.
    *5. When it is done waiting strollTime is reset to 0f.
   */
    public void Move(Enemy enemy)
    {
        //1
        enemy.body.mSpeed.x = enemy.mMovingSpeed;
        //2
        if (moveTimer < moveDuration)
        {
            if (enemy.body.mPS.pushesLeftTile || enemy.body.mPS.pushesRightTile)
            {
                enemy.mMovingSpeed *= -1;

            }
            moveTimer += Time.deltaTime;
            return;
        }
        //3-4
        else if (moveTimer > moveDuration && moveTimer < wait)
        {
            //Movement Cooldown reached .
            //reset strollTime.
            enemy.body.mSpeed.x = 0f;
            moveTimer += Time.deltaTime;
            return;
        }
        //5
        else if (moveTimer > wait)
        {
            moveTimer = 0f;
            return;
        }
    }

    public void Move(Enemy entity, Entity target, int direction)
    {
        //1-2
        if (moveTimer < moveDuration)
        {
            //If we have a target move in it's direction.
            if (target != null)
            {
                entity.body.mSpeed.x = entity.mMovingSpeed * direction;
            }

            moveTimer += Time.deltaTime;
            return;
        }
        //3-4
        else if (moveTimer > moveDuration && moveTimer < wait)
        {
            //Debug.Log("Resting");
            //Movement Cooldown reached .
            //reset strollTime.
            entity.body.mSpeed.x = 0f;
            moveTimer += Time.deltaTime;
            return;
        }
        //5
        else if (moveTimer > wait)
        {
            //Debug.Log("Done Resting.");
            moveTimer = 0f;
            return;
        }
    }

    public void Jump(Enemy enemy, int direction)
    {

        //If we have a target, and we arent jumping.
        if (enemy.Target != null && !jumping && jumpDuration == 0)
        {
            enemy.Body.mSpeed.y = jumpSpeed * direction;
            jumping = true;
        }

        //If we have initiated a jump, add force to our mSpeed y.
        if (jumping && jumpDuration <= jumpTimer)
        {
            jumpDuration += Time.deltaTime;
        }
        else if (jumping && jumpDuration >= jumpTimer)
        {
            jumpDuration = 0;
            jumping = false;
        }
    }

    public void CheckForTargets(Enemy enemy)
    {
        //First enemies check sight
        if (enemy.Sight.mEntitiesInSight != null)
        {
            foreach (Entity entity in enemy.Sight.mEntitiesInSight)
            {
                if (entity is Player && enemy.Hostility == Hostility.Hostile)
                {
                    enemy.Target = entity;
                    break;
                }
            }
        }
    }

    public void EnemyBehaviourLoop(Enemy enemy)
    {
        CheckForTargets(enemy);

        if (enemy.Target != null)
        {
            if (enemy.Target.Position.x > enemy.Body.mPosition.x)
            {
                Move(enemy,enemy.Target, 1);
                if (enemy.Target.Position.y - enemy.Body.mPosition.y > 30)
                {
                    Jump(enemy, 1);
                }

            }
            else if (enemy.Target.Position.x < enemy.Body.mPosition.x)
            {
                //Debug.Log("Target: " + target.Position.y + "Slime: " + this.Body.mPosition.y);
                Move(enemy,enemy.Target, -1);
                if (enemy.Target.Position.y - enemy.Body.mPosition.y > 30)
                {
                    Debug.Log("jumping!");
                    Jump(enemy, 1);
                }
            }
        }
        else
        {
            Move(enemy);
        }

        //Enemy attack a target if it is hostile towards it.
        if(enemy.Target != null)
        {
            EnemyAttack(enemy);
        }
        

    }

    public void EnemyAttack(Enemy enemy)
    {

            //If target is standing close to Entity
            if (Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) < 20 && Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) > -20 && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) < 30 && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) > -30)
            {
                //If target is to the left of the Entity && Target has an attack...
                if (enemy.Target.Position.x < enemy.Body.mPosition.x && enemy.mAttackManager.AttackList != null)
                {
                    //Check if target can make a close range attack.
                    foreach (MeleeAttack attack in enemy.mAttackManager.AttackList)
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
                else if (enemy.Target.Position.x > enemy.Body.mPosition.x && enemy.mAttackManager.AttackList != null)
                {

                    //Check if target can make a close range attack.
                    foreach (MeleeAttack attack in enemy.mAttackManager.AttackList)
                    {
                        if (!attack.mIsActive)
                        {
                            attack.hitbox.mAABB.OffsetX = Mathf.Abs(attack.hitbox.mAABB.OffsetX);
                        }
                    }
                }
                //Attack
                enemy.mAttackManager.AttackList[0].Activate();
            }


            enemy.mAttackManager.UpdateAttacks();

    }


    public State State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }

}
