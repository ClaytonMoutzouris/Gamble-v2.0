using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityDirection { Left = -1, Right = 1 };
public enum Alignment { Player, Neutral, Enemy };

[System.Serializable]
public class Entity {

    public string Name;
    public EntityType mEntityType;
    public List<EntityType> mCollidesWith;
    public float mMovingSpeed;
    public Vector2 Position;
    public EntityDirection mDirection = EntityDirection.Right;
    #region HiddenInInspector
    private EntityPrototype prototype;
    private EntityRenderer renderer;
    private PhysicsBody body;
    private LevelManager game;
    private MapManager map;
    [HideInInspector]
    public bool mToRemove = false;
    [HideInInspector]
    public int mUpdateId = -1; //The order in the update list that the entity is updated, this matters for things like mounting
    #endregion
    protected bool isSpawned = false;
    public bool ignoreTilemap = false;

    //This might be better served as an entity type matrix rather than collision type




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

    public Vector3 Scale
    {
        get
        {
            //Ok this is pretty nasty, ill try and fix this
            return body.mAABB.Scale;
        }
    }

    public EntityRenderer Renderer
    {
        get
        {
            return renderer;
        }

        set
        {
            renderer = value;
        }
    }

    public LevelManager Game
    {
        get
        {
            return game;
        }

        set
        {
            game = value;
        }
    }

    public MapManager Map
    {
        get
        {
            return map;
        }

        set
        {
            map = value;
        }
    }

    public bool IsSpawned
    {
        get
        {
            return isSpawned;
        }

        set
        {
            isSpawned = value;
        }
    }

    #endregion


    public Entity(EntityPrototype proto)
    {
        prototype = proto;
        mEntityType = prototype.entityType;
        Game = LevelManager.instance;
        Map = Game.mMap;
        //mRenderer = GameObject.Instantiate<EntityRenderer>();
        mCollidesWith = new List<EntityType>();
        
        //if (colorPallete != null && colorPallete.Count > 0)
        //ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);

        mUpdateId = Game.AddToUpdateList(this);
    }

    public virtual void Spawn(Vector2 spawnPoint)
    {
        GameObject gameObject = GameObject.Instantiate(Resources.Load("Prefabs/EntityRenderer")) as GameObject;
        Renderer = gameObject.GetComponent<EntityRenderer>();
        Renderer.SetEntity(this);

        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }

        Position = spawnPoint + body.mOffset;
        Renderer.Sprite.sortingLayerName = prototype.sortingLayer.ToString();
        Renderer.Draw();
        Body.UpdatePosition();
        isSpawned = true;
    }

    public virtual void EntityUpdate()
    {


        Body.UpdatePhysics();
        Renderer.Draw();

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
        
        Renderer.Draw();
        
    }

    public virtual void Die()
    {
        mToRemove = true;
        Body.mState = ColliderState.Closed;
    }

    public virtual void Destroy()
    {
        mToRemove = true;
        Body.mState = ColliderState.Closed;
    }

    public virtual void ActuallyDie()
    {

        //before we remove it from the update list, we have to remove it from the update areas
        CollisionManager.RemoveObjectFromAreas(Body);
        Game.RemoveFromUpdateList(this);
        if (Renderer != null)
        {
            UnityEngine.Object.Destroy(Renderer.gameObject);
        }
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
        FloatingText floatingText = GameObject.Instantiate(Resources.Load<FloatingText>("Prefabs/UI/FloatingText") as FloatingText, Position, Quaternion.identity);
        floatingText.SetOffset(Vector3.up * (Body.mAABB.HalfSize.y * 2 + 7));
        floatingText.GetComponent<TextMesh>().text = "" + damage;
        floatingText.GetComponent<TextMesh>().color = color;
    }
}






