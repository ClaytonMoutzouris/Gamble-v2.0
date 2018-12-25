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

    [SerializeField]
    public CustomCollider2D mHurtBox;

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

    public Vector3 Position
    {
        get
        {
            return body.mPosition;
        }
    }

    #endregion


    public AttackManager mAttackManager;

    void OnDrawGizmos()
    {
        DrawHitboxGizmos();
    }

    protected void DrawHitboxGizmos()
    {
        //calculate the position of the aabb's center
        var aabbPos = mHurtBox.mAABB.Center;

        //draw the aabb rectangle
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(aabbPos, mHurtBox.mAABB.HalfSize * 2.0f);


    }

    private void Awake()
    {
        mAttackManager = GetComponent<AttackManager>();
        if (mAttackManager == null)
            mAttackManager = gameObject.AddComponent<AttackManager>();

        mAttackManager.mEntity = this;


        body = GetComponent<PhysicsObject>();

        mHurtBox.mEntity = this;
        mHurtBox.colliderType = ColliderType.Hurtbox;

        //body.mTransform = transform;
    }

    public virtual void EntityInit()
    {
        mGame = Game.instance;
        mMap = mGame.mMap;

        if(colorPallete != null && colorPallete.Count > 0)
        ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);

        body.ObjectInit(this);

        mHurtBox.mAABB.baseHalfSize = Body.mCollider.mAABB.HalfSize;
        mHurtBox.mAABB.HalfSize = Body.mCollider.mAABB.HalfSize;
        mHurtBox.UpdatePosition();

        mUpdateId = mGame.AddToUpdateList(this);

        //mEnemyType = EnemyType.Slime;
    }

    public virtual void EntityUpdate()
    {
        body.UpdatePhysics();
        mHurtBox.UpdatePosition();

    }

    public virtual void SecondUpdate()
    {
        Body.UpdatePhysicsP2();
        mAttackManager.SecondUpdate();
    }

    public void Shoot(Bullet prefab)
    {
        Bullet temp = Instantiate(prefab, body.mCollider.mAABB.Center, Quaternion.identity);
        temp.EntityInit();
        temp.direction = new Vector2(body.mCollider.mAABB.ScaleX, 0);
    }

    public void Die()
    {
        Debug.Log(name + " has died");

        mToRemove = true;
        mHurtBox.mState = ColliderState.Closed;
        mHurtBox.mCollisions.Clear();
    }
}



public abstract class Enemy : Entity
{

    public EnemyType mEnemyType;


}


