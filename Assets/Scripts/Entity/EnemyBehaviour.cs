using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Idle, Moving, Jumping, Attacking, Aggrivated, Attack1, Attack2, Attack3 };
public enum TargetRange {  Close, Near, Far , OutOfRange};
public class EnemyBehaviour : MonoBehaviour
{

    public Enemy mEnemy;
    public State state = State.Idle;
    public TargetRange range = TargetRange.OutOfRange;

    public bool canMove;
    public bool canJump;
    public bool canAttack;

    public bool isOnCooldown;
    public bool isMoving;
    public bool isJumping;
    public bool isAttacking;

    public float cooldownDuration;
    public float moveDuration;
    public float jumpDuration;

    public float moveTimer;
    public float jumpTimer;
    public float cooldownTimer;

    public float closeRange;
    public float nearRange;
    public float farRange;

    public float jumpSpeed;

    public int direction;

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

    public void EnemyBehaviourLoop(Enemy enemy)
    {
        CheckForTargets(enemy);

        if (isOnCooldown)
        {
            Wait(enemy);
            return;
        }

        if (!isAttacking)
        {
            Move(enemy);
        }

        if (TargetInCombatDistance(enemy))
        {
            if (canEnemyAttack(enemy))
            {
                EnemyAttack(enemy);
                return;
            }
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

/*1.State is IDLE.
*2.Horizontal speed is 0.
*3.If cooldownTimer exceeds cooldownDuration, reset states/booleans.
*4.Otherwise cooldownTimer counts up.
*/
    public void Wait(Enemy enemy)
    {
        //1
        state = State.Idle;
        //2
        enemy.Body.mSpeed.x = 0f;
        //3
        if (cooldownTimer >= cooldownDuration)
        {
            cooldownTimer = 0f;
            isOnCooldown = false;
            isAttacking = false;
            return;
            //Wait finished.
        }
        //4
        cooldownTimer += Time.deltaTime;
        return;
    }

 /*
 * 1.Change speed from 0.
 * 2.If we have a target, move toward it.
 * 3.Otherwise, just move until we touch a wall. (Left or Right)
 * 4.Update Movement timer.
 * 5.If movement is used, isOnCooldown = true.
*/
    public void Move(Enemy enemy)
    {
        //1
        if (enemy.body.mSpeed.x == 0)
        {
            enemy.body.mSpeed.x = enemy.mMovingSpeed * direction;
        }
        //2
        if (enemy.Target != null)
        {
            if (TargetToRight(enemy))
            {
                direction = 1;
                enemy.body.mSpeed.x = Mathf.Abs(enemy.mMovingSpeed);
                //Is enemy higher then us?
                if (enemy.Target.Position.y - enemy.Body.mPosition.y > 30 || isJumping)
                {
                    Jump(enemy);
                }
            }
            else if (direction != -1 && TargetToLeft(enemy))
            {
                direction = -1;
                enemy.body.mSpeed.x = enemy.mMovingSpeed * direction;
                //Is enemy higher than us?
                if (enemy.Target.Position.y - enemy.Body.mPosition.y > 30 || isJumping)
                {
                    Jump(enemy);
                }
            }
        } 
        //3
        else
        {
            //If we touch somthing to our right or left or right.
            if (enemy.body.mPS.pushedLeftTile)
            {
                //Change direction.
                direction = 1;
                enemy.body.mSpeed.x = Mathf.Abs(enemy.mMovingSpeed);

            }
            else if (enemy.body.mPS.pushedRightTile && direction != -1)
            {
                direction = -1;
                enemy.body.mSpeed.x = enemy.mMovingSpeed * direction;
            }
        }
        //4
        if (moveTimer < moveDuration)
        {
            state = State.Moving;
            moveTimer += Time.deltaTime;
            return;
        }
        //5
        else if (moveTimer > moveDuration)
        {
            isOnCooldown = true;
            moveTimer = 0f;
            return;
        }
    }

    public void Jump(Enemy enemy)
    {
        //If we have a target, and we arent jumping...JUMP!
        if (enemy.Target != null && !isJumping && jumpTimer == 0f)
        {
            enemy.Body.mSpeed.y = jumpSpeed;
            isJumping = true;
            return;
        }

        if (isJumping && jumpTimer <= jumpDuration)
        {
            jumpTimer += Time.deltaTime;
            return;
        }
        else if (isJumping && jumpTimer >= jumpDuration)
        {
            jumpTimer = 0;
            isJumping = false;
            return;
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

    public bool canEnemyAttack(Enemy enemy)
    {
        if (enemy.mAttackManager.AttackList != null)
        {
            return true;
        }

        return false;
    }

    public bool TargetInCombatDistance(Enemy enemy)
    {

        if (enemy.Target != null)
        {
            if (Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) < closeRange && Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) > -closeRange && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) < 30 && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) > -30)
            {
                range = TargetRange.Close;
                return true;
            }

            if (Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) < nearRange && Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) > -nearRange && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) < 30 && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) > -30)
            {
                range = TargetRange.Near;
                return true;
            }

            if (Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) < farRange && Mathf.Abs(enemy.Target.Position.x) - Mathf.Abs(enemy.Body.mPosition.x) > -farRange && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) < 30 && Mathf.Abs(enemy.Target.Position.y) - Mathf.Abs(enemy.Body.mPosition.y) > -30)
            {
                range = TargetRange.Far;
                return true;
            }

            range = TargetRange.OutOfRange;
            return false;

        }

        return false;
    }
/*
 *1.If we are attacking, loop through attacks to check if any are active. If not, we aren't attacking.
 *2.If we arent attacking, loop through attacks to see if any are in range.
 *3.Set the attacks direction.
 *4.Stop Moving.
 *5.Activate Attack.
 */
    public void EnemyAttack(Enemy enemy)
    {
        //1
        if (isAttacking)
        {
            for(int i = 0; i < enemy.mAttackManager.AttackList.Count; i++)
            {
                if (enemy.mAttackManager.AttackList[i].mIsActive)
                {
                    return;
                }
            }
            isAttacking = false;
        }
        //2
        if (!isAttacking)
        {
            for (int i = 0; i < enemy.mAttackManager.AttackList.Count; i++)
            {
                if ((int)enemy.mAttackManager.AttackList[i].range == (int)range && !enemy.mAttackManager.AttackList[i].onCooldown)
                {
                    isAttacking = true;
                    //3
                    SetAttackHitboxDirection(enemy,i);
                    //4
                    enemy.body.mSpeed.x = 0f;
                    //5
                    enemy.mAttackManager.AttackList[i].Activate();
                    return;
                }
            }
        }
    }


    public void SetAttackHitboxDirection(Enemy enemy, int i)
    {
        MeleeAttack attack = (MeleeAttack)enemy.mAttackManager.AttackList[i];
        //If the target is to the left of the Entity.
        if (TargetToLeft(enemy))
        {
            if (!attack.mIsActive)
            {
                //Check if the hitbox has already set OffsetX to face to the left.
                if (attack.hitbox.mAABB.OffsetX > 0)
                    attack.hitbox.mAABB.OffsetX = attack.hitbox.mAABB.OffsetX * -1;
            }
        }
        //If target is to the right of the Entity.
        else if (TargetToRight(enemy))
        {
            if (!attack.mIsActive)
            {
                attack.hitbox.mAABB.OffsetX = Mathf.Abs(attack.hitbox.mAABB.OffsetX);
            }
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
