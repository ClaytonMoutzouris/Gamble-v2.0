using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
public abstract class Entity : MonoBehaviour {

    public float mMovingSpeed;
    public bool mToRemove = false;
    public int mUpdateId = -1;
    public Game mGame;
    public MapManager mMap;
    public bool mDoubleJump = true;

    public Animator mAnimator;
    public List<Color> colorPallete;

    //Every entity has a body, it exists in the world
    //Whether the body actually interacts with anything, thats up to the body
    protected PhysicsObject body;
    public Hurtbox mHurtBox;

    public EntityData EntityData;

    #region Accesors
    public PhysicsObject Body
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

    #endregion


    public AttackManager mAttackManager;


    private void Awake()
    {
        mAttackManager = GetComponent<AttackManager>();
        if (mAttackManager == null)
            mAttackManager = gameObject.AddComponent<AttackManager>();

        mAttackManager.mEntity = this;


        body = GetComponent<PhysicsObject>();
        mHurtBox = GetComponent<Hurtbox>();
        if(mHurtBox == null)
        {
            mHurtBox = gameObject.AddComponent<Hurtbox>();
        }

        mHurtBox.mEntity = this;
        //if()
        body.mTransform = transform;
    }

    public virtual void EntityInit()
    {
        mGame = Game.instance;
        mMap = mGame.mMap;

        if(colorPallete != null && colorPallete.Count > 0)
        ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);

        body.ObjectInit(this);

        mHurtBox.collider.halfSize = body.mAABB.halfSize;
        mHurtBox.collider.Offset = body.mOffset;

        mUpdateId = mGame.AddToUpdateList(this);

        //mEnemyType = EnemyType.Slime;
    }

    public virtual void EntityUpdate()
    {
        body.UpdatePhysics();
        mHurtBox.collider.Center = body.mAABB.Center;

    }

    public void Shoot(Bullet prefab)
    {
        Bullet temp = Instantiate(prefab, body.mAABB.Center, Quaternion.identity);
        temp.EntityInit();
        temp.direction = new Vector2(body.ScaleX, 0);
    }
}



public abstract class Enemy : Entity
{

    public EnemyType mEnemyType;


}


