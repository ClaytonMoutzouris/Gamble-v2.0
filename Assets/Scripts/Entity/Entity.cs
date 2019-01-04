using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {

    //The speed of the entity when it moves, probably not needed by all entities
    public float mMovingSpeed;
    //When we want to remove an entity, either from dying or something else, we must flag it for removal.
    //This is because we can't just delete an object while iterating through a list, or it will cause problems
    public bool mToRemove = false;
    //The order in the update list that the entity is updated, this matters for things like mounting
    public int mUpdateId = -1;
    public Game mGame;
    public MapManager mMap;
    //Probably all entities will have an animator, even if it doesnt need it
    public Animator mAnimator;
    //This is for changing the color pallete of an entity, right now only the players use this but will be useful for making different enemies with the same behaviours (Green slime, red slime, etc.)
    public List<Color> colorPallete;


    //Every entity has a body, it exists in the world
    //Whether the body actually interacts with anything, thats up to the body
    [SerializeField]
    protected PhysicsBody body;

    public EntityType mEntityType;

    //This might be better served as an entity type matrix rather than collision type
    public List<EntityType> mCollidesWith;

    //Not currently in use
    public EntityData EntityData;

    #region Accesors
    public PhysicsBody Body
    {
        get
        {
            return body;
        }

        set
        {
            body = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            //The bodies position + its offset so we get the middle (right now sprites are bottom aligned, if we changed to center we would need to change this among other things)
            return body.mPosition + (Vector2)body.mAABB.Offset;
        }
    }

    public Vector3 Scale
    {
        get
        {
            //Ok this is pretty nasty, ill try and fix this
            return body.mAABB.Scale;
        }
    }

    #endregion

    public virtual void OnDrawGizmos()
    {
        if (body != null)
        {
            Gizmos.color = new Color(0, 0, 1, 0.35f);
            Gizmos.DrawCube(body.mAABB.Center, body.mAABB.HalfSize * 2);
        }
    }

    public virtual void EntityInit()
    {
        mGame = Game.instance;
        mMap = mGame.mMap;

        if(colorPallete != null && colorPallete.Count > 0)
        ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);

        mUpdateId = mGame.AddToUpdateList(this);

        //mEnemyType = EnemyType.Slime;
    }

    public virtual void EntityUpdate()
    {
        body.UpdatePhysics();

        //Update the areas of the the colliders
        CollisionManager.UpdateAreas(Body);

        //After updating the areas, clear all the collisions for this frame so that we dont have any remnants from last frame
        Body.mCollisions.Clear();


        //Right now it works without doing this here, but ill leave it until im sure its fine to remove
        //mHurtBox.UpdatePosition();

    }

    public virtual void SecondUpdate()
    {
        Body.UpdatePhysicsP2();
    }

    public void Shoot(Bullet prefab, Attack attack)
    {
        Bullet temp = Instantiate(prefab, body.mAABB.Center, Quaternion.identity);
        temp.EntityInit();
        temp.Attack = attack;
        temp.Owner = this;
        temp.direction = new Vector2(body.mAABB.ScaleX, 0);
    }

    public virtual void Die()
    {
        Debug.Log(name + " has died");

        mToRemove = true;

    }

    public virtual void ActuallyDie()
    {

        //before we remove it from the update list, we have to remove it from the update areas
        CollisionManager.RemoveObjectFromAreas(Body);


        mGame.RemoveFromUpdateList(this);

        //Finally, kill this object forever
        Destroy(gameObject);

    }
}






