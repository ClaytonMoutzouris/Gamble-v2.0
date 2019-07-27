using UnityEngine;

public class Tree : Entity
{
    public Tree(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, new Vector2(8, 8), new Vector2(0, 8)))
        {
            mIsKinematic = false
        };
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        Renderer.SetSprite(Resources.Load<Sprite>("Sprites/Objects/Tree") as Sprite);

    }
}
