using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy, IContactTrigger
{
    Direction direction = Direction.Left;

    public Crawler(EnemyPrototype proto) : base(proto)
    {
        mEnemyState = EnemyState.Moving;
        
        direction = Direction.Left;
        spikeProtection = true;
    }

    public void Contact(Entity entity)
    {
        if(entity is IHurtable hurtable)
        {
            if(!mAttackManager.meleeAttacks[0].mIsActive && !mAttackManager.meleeAttacks[0].OnCooldown())
            {
                mAttackManager.meleeAttacks[0].Activate();

            }
        }
    }

    public override void EntityUpdate()
    {

        /*
        if (!Body.mPS.pushesBottom
            && !Body.mPS.pushesTop
            && !Body.mPS.pushesRight
            && !Body.mPS.pushesLeft)
        {
            Body.mIgnoresGravity = false;
            
        }
        else
        {
            Body.mIgnoresGravity = true;

        }
        */

        switch (mEnemyState)
        {

            case EnemyState.Idle:

                break;
            case EnemyState.Moving:

                switch (direction)
                {
                    case Direction.Left:

                        if(Body.mPS.pushesLeft)
                        {
                            gravityVector = new Vector2(-1, 0);

                            if (Body.mPS.pushesBottom)
                            {
                                direction = Direction.Up;
                                break;
                            }

                            if (Body.mPS.pushesTop)
                            {
                                direction = Direction.Down;

                                break;
                            }
                            direction = Direction.Up;
                            break;

                        }

                        if (!Body.mPS.pushesBottom && Body.mPS.pushedBottom)
                        {
                            direction = Direction.Down;
                            gravityVector = new Vector2(1, 0);

                            break;
                        }

                        if (!Body.mPS.pushesTop && Body.mPS.pushedTop)
                        {
                            direction = Direction.Up;
                            gravityVector = new Vector2(1, 0);

                            break;
                        }


                        break;
                    case Direction.Right:

                        if (Body.mPS.pushesRight)
                        {
                            gravityVector = new Vector2(1, 0);

                            if (Body.mPS.pushesBottom)
                            {
                                direction = Direction.Up;
                                break;
                            }

                            if (Body.mPS.pushesTop)
                            {
                                direction = Direction.Down;
                                break;
                            }

                            direction = Direction.Up;
                            break;
                        }

                        if(!Body.mPS.pushesBottom && Body.mPS.pushedBottom)
                        {
                            direction = Direction.Down;
                            gravityVector = new Vector2(-1, 0);

                            break;
                        }

                        if (!Body.mPS.pushesTop && Body.mPS.pushedTop)
                        {
                            direction = Direction.Up;
                            gravityVector = new Vector2(-1, 0);

                            break;
                        }

                        break;
                    case Direction.Down:


                        if (Body.mPS.pushesBottom)
                        {
                            gravityVector = new Vector2(0, -1);

                            if (Body.mPS.pushesRight)
                            {
                                direction = Direction.Left;

                                break;
                            }

                            if (Body.mPS.pushesLeft)
                            {
                                direction = Direction.Right;

                                break;
                            }

                            direction = Direction.Right;

                            break;
                        }


                        if (!Body.mPS.pushesLeft && Body.mPS.pushedLeft)
                        {
                            direction = Direction.Left;
                            gravityVector = new Vector2(1, 0);

                            break;

                        }

                        if (!Body.mPS.pushesRight && Body.mPS.pushedRight)
                        {
                            direction = Direction.Right;
                            gravityVector = new Vector2(1, 0);

                            break;

                        }

                        if (!Body.mPS.pushesLeft && !Body.mPS.pushesRight)
                        {
                            direction = Direction.Right;
                            gravityVector = new Vector2(0, -1);
                            break;
                        }

                        break;
                    case Direction.Up:

                        if (Body.mPS.pushesTop)
                        {
                            gravityVector = new Vector2(0, 1);
                            if (Body.mPS.pushesRight)
                            {
                                direction = Direction.Left;
                                break;
                            }

                            if (Body.mPS.pushesLeft)
                            {
                                direction = Direction.Right;
                                break;
                            }

                            direction = Direction.Left;
                            break;
                        }

                        if (!Body.mPS.pushesLeft && Body.mPS.pushedLeft)
                        {
                            direction = Direction.Left;
                            gravityVector = new Vector2(0, -1);

                            break;
                        }

                        if (!Body.mPS.pushesRight && Body.mPS.pushedRight)
                        {
                            direction = Direction.Right;
                            gravityVector = new Vector2(0, -1);

                            break;

                        }

                        if(!Body.mPS.pushesLeft && !Body.mPS.pushesRight)
                        {
                            direction = Direction.Left;
                            gravityVector = new Vector2(0, -1);
                            break;
                        }

                        break;
                }


                break;
            case EnemyState.Jumping:
                break;
        }

        //if (Body.mIgnoresGravity)
       // {
            switch (direction)
            {
                case Direction.Left:
                    Body.mSpeed = new Vector2(-mMovingSpeed, 0);
                    Renderer.Sprite.flipX = true;
                    break;
                case Direction.Right:
                    Body.mSpeed = new Vector2(mMovingSpeed, 0);
                    Renderer.Sprite.flipX = false;
                    break;
                case Direction.Down:
                    Body.mSpeed = new Vector2(0, -mMovingSpeed);
                    Renderer.Sprite.flipX = true;
                    break;
                case Direction.Up:
                    Body.mSpeed = new Vector2(0, mMovingSpeed);
                    Renderer.Sprite.flipX = true;
                    break;
           }

            RotateToDirection();
        //}

        base.EntityUpdate();

    }

    public void RotateToDirection()
    {
        int angle = 0;

        switch (direction)
        {
            case Direction.Left:
                Renderer.Sprite.flipX = true;
                angle = 0;
                break;
            case Direction.Right:
                Renderer.Sprite.flipX = false;
                angle = 0;
                break;
            case Direction.Down:
                Renderer.Sprite.flipX = true;
                angle = 90;
                break;
            case Direction.Up:
                Renderer.Sprite.flipX = false;
                angle = 90;
                break;
        }
        Renderer.transform.SetPositionAndRotation(Renderer.transform.position, Quaternion.Euler(0, 0, -angle));

    }

}
