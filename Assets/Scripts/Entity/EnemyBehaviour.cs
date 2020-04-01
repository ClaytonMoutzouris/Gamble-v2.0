using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Idle, Moving, Jumping, Attacking, Aggrivated, Attack1, Attack2, Attack3 };

public static class EnemyBehaviour
{

    public static void CheckForTargets(Enemy enemy)
    {
        enemy.Target = null;
        if (enemy.Sight.mEntitiesInSight != null)
        {
            foreach (Entity entity in enemy.Sight.mEntitiesInSight)
            {
                if (entity is Player)
                {
                    if (!((Player)entity).IsDead)
                    {
                        enemy.Target = (Player)entity;
                        enemy.Hostility = Hostility.Hostile;
                        break;
                    }
                }
            }
        }
    }

    public static bool TargetInRange(Enemy enemy, Entity target, float range)
    {
        if((enemy.Position - target.Position).magnitude < range)
        {
            return true;

        }

        return false;
    }

    public static IEnumerator Wait(Enemy enemy, float waitTime, EnemyState returnState)
    { 
        enemy.mEnemyState = EnemyState.Idle;
        enemy.Body.mSpeed.x = 0f;

        yield return new WaitForSeconds(waitTime);

        enemy.mEnemyState = returnState;
    }

    public static void Jump(Enemy enemy, float jumpSpeed)
    {
        //If they can't proceed horizontally, try jumping.
        if (enemy.Body.mPS.pushesRightTile && enemy.Body.mPS.pushesBottom)
        {
            enemy.Body.mSpeed.y = jumpSpeed;
            enemy.mDirection = EntityDirection.Right;
            enemy.mEnemyState = EnemyState.Jumping;
        }
        else
        {
            enemy.Body.mSpeed.y = jumpSpeed;
            enemy.mDirection = EntityDirection.Right;
            enemy.mEnemyState = EnemyState.Jumping;
        }
    }

    public static void MoveHostile(Enemy enemy)
    {
        CheckForTargets(enemy);
        if (enemy.Target != null && !enemy.Target.IsDead)
        {
            //Replace this with pathfinding to the target

            if (EnemyBehaviour.TargetInRange(enemy, enemy.Target, enemy.closeRange))
            {
                if (!enemy.mAttackManager.meleeAttacks[0].onCooldown)
                {
                    //Renderer.SetAnimState("Slime_Attack");
                    EnemyBehaviour.MeleeAttack(enemy, 0);
                    EnemyBehaviour.Wait(enemy, 2, EnemyState.Idle);
                }
                //StartCoroutine(EnemyBehaviour.Wait(this, mAttackManager.AttackList[0].duration + 2, EnemyState.Moving));
            }
            else
            {
                if (enemy.Target.Position.x > enemy.Position.x)
                {
                    //If they can't proceed horizontally, try jumping.
                    if (enemy.Body.mPS.pushesRightTile && enemy.Body.mPS.pushesBottom)
                    {
                        EnemyBehaviour.Jump(enemy, enemy.jumpHeight);
                    }
                    //body.mSpeed.x = mMovingSpeed;
                    enemy.mDirection = EntityDirection.Right;
                }
                else
                {
                    if (enemy.Body.mPS.pushesLeftTile && enemy.Body.mPS.pushesBottom)
                    {
                        EnemyBehaviour.Jump(enemy, enemy.jumpHeight);
                    }
                    enemy.mDirection = EntityDirection.Left;
                }

                if (!EnemyBehaviour.TargetInRange(enemy, enemy.Target, enemy.midRange))
                {   
                    EnemyBehaviour.MoveHorizontal(enemy);
                }
            }

        }
        else
        {
            if (enemy.Body.mPS.pushedLeftTile)
            {
                //Renderer.Sprite.flipX = true;
                enemy.mDirection = EntityDirection.Right;

            }
            else if (enemy.Body.mPS.pushedRightTile)
            {
                //Renderer.Sprite.flipX = true;
                enemy.mDirection = EntityDirection.Left;
            }

            enemy.Body.mSpeed.x = enemy.mMovingSpeed;
            EnemyBehaviour.MoveHorizontal(enemy);

        }
    }

    public static void Move(Enemy enemy)
    {
        

    }

    public static void MoveHorizontal(Enemy enemy)
    {
        if((int)enemy.mDirection == -1)
        {
            enemy.Renderer.Sprite.flipX = false;
        }
        else
        {
            enemy.Renderer.Sprite.flipX = true;
        }
        enemy.Body.mSpeed.x = enemy.mMovingSpeed * (int)enemy.mDirection;

    }

    public static void MoveVertical(Enemy enemy)
    {
        //enemy.Body.mSpeed.y = enemy.mMovingSpeed * (int)enemy.mDirection;
    }

    public static void MoveAway(Enemy enemy, Entity target)
    {
        if(enemy.Position.x > target.Position.x)
        {
            enemy.Body.mSpeed.x = Mathf.Abs(enemy.mMovingSpeed);
        }
        else
        {
            enemy.Body.mSpeed.x = enemy.mMovingSpeed * -1;
        }
    }

    public static void RangedAttack(Enemy enemy, int index)
    {
        enemy.mAttackManager.rangedAttacks[index].Activate();
        enemy.mEnemyState = EnemyState.Attacking;
        //enemy.Body.mSpeed.x = 0;
    }

    public static void MeleeAttack(Enemy enemy, int index)
    {

        enemy.mAttackManager.meleeAttacks[index].Activate();
        enemy.mEnemyState = EnemyState.Attacking;
        enemy.Body.mSpeed.x = 0;
    }

    public static void SpawnMinion(Enemy enemy)
    {

    }

}
