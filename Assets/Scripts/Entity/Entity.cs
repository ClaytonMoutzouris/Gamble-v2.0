using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    public EntityType mEntityType;
    public List<EntityType> mCollidesWith;
    public List<Color> colorPallete;
    public float mMovingSpeed;
    public Vector2 BodySize;
    #region HiddenInInspector
    public PhysicsBody body;
    [HideInInspector]
    public AudioSource mAudioSource;
    [HideInInspector]
    public Game mGame;
    [HideInInspector]
    public MapManager mMap;
    [HideInInspector]
    public Animator mAnimator;
    [HideInInspector]
    public bool mToRemove = false;
    [HideInInspector]
    public int mUpdateId = -1; //The order in the update list that the entity is updated, this matters for things like mounting
    #endregion

    //This might be better served as an entity type matrix rather than collision type

    //Not currently in use
    [HideInInspector]
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

        mAnimator = GetComponent<Animator>();

        mAudioSource = GetComponent<AudioSource>();

        if (colorPallete != null && colorPallete.Count > 0)
        ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);

        mUpdateId = mGame.AddToUpdateList(this);

        //Set the physicsBody
        Body = new PhysicsBody(this, new CustomAABB(transform.position, BodySize, new Vector2(0, BodySize.y), new Vector3(1, 1, 1)));

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

    public virtual void Shoot(Bullet prefab, Attack attack)
    {
        Bullet temp = Instantiate(prefab, body.mAABB.Center, Quaternion.identity);
        temp.EntityInit();
        temp.Attack = attack;
        temp.Owner = this;
        temp.SetInitialDirection(new Vector2(body.mAABB.ScaleX, 0));

    }

    public virtual void Die()
    {
        Debug.Log(name + " has died");

        mToRemove = true;
        Body.mState = ColliderState.Closed;
    }

    public virtual void ActuallyDie()
    {

        //before we remove it from the update list, we have to remove it from the update areas
        CollisionManager.RemoveObjectFromAreas(Body);


        mGame.RemoveFromUpdateList(this);

        //Finally, kill this object forever
        Destroy(gameObject);

    }

    public virtual void Crush()
    {
        //Kinematic things cant be spiked or crushed
        if (Body.mIsKinematic)
            return;
        //Vector2 temp = mMap.GetMapTilePosition(mMap.mWidth/2 , mMap.mHeight / 2 );
        /*
        mEntity.transform.position = mMap.GetMapTilePosition(mMap.mMapData.startTile);
        mPosition = mEntity.transform.position;
        mAABB.Center = mPosition;
        mPS.Reset();
        */

        

    }

    public virtual void ShowFloatingText(int damage, Color color)
    {
        FloatingText floatingText = Instantiate(Resources.Load<FloatingText>("Prefabs/UI/FloatingText") as FloatingText, transform.position, Quaternion.identity);
        floatingText.SetOffset(Vector3.up * (Body.mAABB.HalfSize.y * 2 + 7));
        floatingText.GetComponent<TextMesh>().text = "" + damage;
        floatingText.GetComponent<TextMesh>().color = color;
    }
}






