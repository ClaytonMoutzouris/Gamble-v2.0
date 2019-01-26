using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Idle, Moving, Jumping, Attacking, Aggrivated, Attack1, Attack2, Attack3 };
public class EnemyBehaviour : MonoBehaviour
{

    public Enemy mEnemy;
    private State state = State.Idle;


    public bool canMove;
    public bool canJump;
    public bool canAttack;

    public bool isWaiting;
    public bool isMoving;
    public bool isJumping;
    public bool isAttacking;

    public float waitDuration;
    public float moveDuration;
    public float jumpDuration;
    public float basicAttackDuration;

    public float moveTimer;
    public float jumpTimer;
    public float waitTimer;
    public float basicAttackTimer;

    public float jumpSpeed;


    public EnemyBehaviour(Enemy enemy)
    {
        mEnemy = enemy;
    }

    void EnemyBehaviourInit()
    {
        canMove = false;
        canJump = false;
        canAttack = false;
        isAttacking = false;

        state = State.Idle;
    }

    public void EnemyBehaviourUpdate(Enemy enemy)
    {
        EnemyBehaviourLoop(enemy);
    }

    public void Wait(Enemy enemy)
    {
        enemy.Body.mSpeed.x = 0f;

        if(waitTimer >= waitDuration)
        {
            waitTimer = 0f;
            isWaiting = false;
            return;
            //Wait finished.
        }

        waitTimer += Time.deltaTime;
        return;
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

        if (moveTimer < moveDuration)
        {

            enemy.body.mSpeed.x = enemy.mMovingSpeed;

            //If we touch somthing to our right or left.
            if (enemy.body.mPS.pushedLeftTile || enemy.body.mPS.pushedRightTile)
            {

                //Change direction.
                enemy.mMovingSpeed *= -1;

            }
            moveTimer += Time.deltaTime;
            return;
        }
        //3-4
        else if (moveTimer > moveDuration)
        {
            isWaiting = true;
            moveTimer = 0f;
            return;
        }
    }

    public void Move(Enemy enemy, Entity target, int direction)
    {
        //1-2
        if (moveTimer < moveDuration)
        {
            //If we have a target move in it's direction.
            if (target != null && isMoving == false)
            {
                isMoving = true;
                enemy.body.mSpeed.x = enemy.mMovingSpeed * direction;
            }

            moveTimer += Time.deltaTime;
            return;
        }
        else if (moveTimer > moveDuration)
        {
            isMoving = false;
            isWaiting = true;
            moveTimer = 0f;
            return;
        }
    }

    public void Jump(Enemy enemy, int direction)
    {

        //If we have a target, and we arent jumping...JUMP!
        if (enemy.Target != null && !isJumping && jumpTimer == 0f)
        {
            enemy.Body.mSpeed.y = jumpSpeed * direction;
            isJumping = true;
        }

        if (isJumping && jumpTimer <= jumpDuration)
        {
            jumpTimer += Time.deltaTime;
        }
        else if (isJumping && jumpTimer >= jumpDuration)
        {
            jumpTimer = 0;
            isJumping = false;
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

        if (isWaiting)
        {
            Wait(enemy);
            return;
        }


        if (enemy.Target != null)
        {

            if (TargetInMeleeDistance(enemy) || isAttacking)
            {
                EnemyAttack(enemy);
                enemy.mAttackManager.UpdateAttacks();
                return;
            }

            if (TargetToRight(enemy))
            {
                state = State.Moving;
                Move(enemy,enemy.Target, 1);
                if (enemy.Target.Position.y - enemy.Body.mPosition.y > 30)
                {
                    state = State.Jumping;
                    Jump(enemy, 1);
                }

            }
            else if (TargetToLeft(enemy))
            {
                state = State.Moving;
                Move(enemy,enemy.Target, -1);
                if (enemy.Target.Position.y - enemy.Body.mPosition.y > 30)
                {
                    state = State.Jumping;
                    Jump(enemy, 1);
                }
            }
        }
        else
        {
            state = State.Moving;
            Move(enemy);
        }


        

    }

    public bool TargetToRight(Enemy enemy)
    {
        if (enemy.Target.Position.x > enemy.Body.mPosition.x)
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }

    public bool TargetInMeleeDistance(Enemy enemy)
    {
        if (Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) < 20 && Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) > -20 && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) < 30 && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) > -30)
        {
            return true;
        }

        return false;
    }

    public bool TargetToLeft(Enemy enemy)
    {
        if (enemy.Target.Position.x < enemy.Body.mPosition.x)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void EnemyAttack(Enemy enemy)
    {
        //If we are already attacking.
        //if (basicAttackTimer < basicAttackDuration && isAttacking)
        if(enemy.mAttackManager.AttackList[0].OnCooldown())
        {
            isAttacking = false;
            return;
        }

        if (isAttacking)
        {
            enemy.mAttackManager.AttackList[0].Activate();
            return;
        }


        /*
        else if(enemy.mAttackManager.AttackList[0].OnCooldown())
        //else if (basicAttackTimer > basicAttackDuration)
        {
            isAttacking = false;
            isWaiting = true;
            basicAttackTimer = 0f;
            enemy.mAttackManager.AttackList[0].Deactivate();
            return;
        }
        */


        //If target is standing close to Entity
        if (!isAttacking)
            {
                //If target is to the left of the Entity && Target has an attack...
                if (TargetToLeft(enemy) && enemy.mAttackManager.AttackList != null)
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
                else if (TargetToRight(enemy) && enemy.mAttackManager.AttackList != null)
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

            
            isAttacking = true;
        }
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
