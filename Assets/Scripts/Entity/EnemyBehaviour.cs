using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Idle, Moving, Jumping, Attacking, Aggrivated, Attack1, Attack2, Attack3 };

public static class EnemyBehaviour
{

    public static void CheckForTargets(Enemy enemy)
    {


        if (enemy.Sight.mEntitiesInSight != null)
        {
            if (enemy.Target != null)
            {
                if (enemy.Target.hostility == enemy.hostility)
                {
                    enemy.Target = null;
                }

                if (enemy.Sight.mEntitiesInSight.Contains(enemy.Target))
                {
                    return;
                }

            }

            foreach (Entity entity in enemy.Sight.mEntitiesInSight)
            {
                if (entity is IHurtable && entity.hostility != enemy.hostility && entity.mEntityType != EntityType.Obstacle && entity.mEntityType != EntityType.Object)
                {
                    if (entity is Player player)
                    {
                        if(player.IsDead)
                        {
                            continue;
                        }
                    }


                    enemy.Target = entity;
                }
                
            }
        } else
        {
            enemy.Target = null;
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

        enemy.Body.mSpeed.y = jumpSpeed;
        enemy.mEnemyState = EnemyState.Jumping;

    }

    public static void Jump(Enemy enemy, float jumpSpeed, Vector2 dir)
    {
        enemy.Body.mSpeed = dir.normalized*jumpSpeed + Vector2.up*jumpSpeed/2;
        enemy.mEnemyState = EnemyState.Jumping;

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
        enemy.Body.mSpeed.x = enemy.GetMovementSpeed() * (int)enemy.mDirection;

    }

    public static void MoveVertical(Enemy enemy)
    {
        //enemy.Body.mSpeed.y = enemy.mMovingSpeed * (int)enemy.mDirection;
    }

    public static void MoveAway(Enemy enemy, Entity target)
    {
        if(enemy.Position.x > target.Position.x)
        {
            enemy.Body.mSpeed.x = Mathf.Abs(enemy.GetMovementSpeed());
            //enemy.Body.mAABB.ScaleX = 1;
        }
        else
        {
            enemy.Body.mSpeed.x = enemy.GetMovementSpeed() * -1;
            //enemy.Body.mAABB.ScaleX = -1;
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
