﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Idle, Moving, Jumping, Attacking, Aggrivated, Attack1, Attack2, Attack3 };

public static class EnemyBehaviour
{

    public static void CheckForTargets(Enemy enemy)
    {
        //First enemies check sight
        if (enemy.Sight.mEntitiesInSight != null)
        {
            foreach (Entity entity in enemy.Sight.mEntitiesInSight)
            {
                if (entity is Player)
                {
                    enemy.Target = entity;
                    break;
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

        enemy.Body.mSpeed.y = jumpSpeed;
        enemy.mEnemyState = EnemyState.Jumping;

    }

    public static void Move(Enemy enemy)
    {


    }

    public static void MoveHorizontal(Enemy enemy)
    {
        enemy.Body.mSpeed.x = enemy.mMovingSpeed * (int)enemy.mDirection;
    }

    public static void MoveVertical(Enemy enemy)
    {
        //enemy.Body.mSpeed.y = enemy.mMovingSpeed * (int)enemy.mDirection;
    }

    public static void MoveAway(Enemy enemy, Entity target)
    {
        if(enemy.body.mPosition.x > target.body.mPosition.x)
        {
            enemy.Body.mSpeed.x = Mathf.Abs(enemy.mMovingSpeed);
        }
        else
        {
            enemy.Body.mSpeed.x = enemy.mMovingSpeed * -1;
        }
    }

    public static void Attack(Enemy enemy, int index)
    {
        enemy.mAttackManager.AttackList[index].Activate();
        enemy.mEnemyState = EnemyState.Attacking;
        enemy.Body.mSpeed.x = 0;
    }


}
