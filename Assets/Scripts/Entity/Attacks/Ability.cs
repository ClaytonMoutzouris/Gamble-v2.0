using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
//Non-Generic Ability class to make dealing with abilities easier
public abstract class Ability
{
    public abstract void Attach(Object attachTo);
    public abstract void Detach(Object detatchFrom);
}

//Generic ability class with specified attachable type and applicaple type
//AttachType and ApplyType have to reference types (classes) because we are relying on side effects.
public abstract class Ability<AttachType, ApplyType> : Ability
    where AttachType : class
    where ApplyType : class
{
    public virtual int Rank { get; set; }

    public Ability(int rank) { Rank = rank; }
    public abstract void Apply(AttachType sender, ApplyType applyTo);
    public abstract void Attach(AttachType attachTo);
    public abstract void Detach(AttachType detachFrom);

    public void Apply(Object sender, Object applyTo)
    {
        Apply((sender as AttachType), (applyTo as ApplyType));
    }
    public override void Attach(object attachTo)
    {
        Attach((attachTo as AttachType));
    }
    public override void Detach(object detatchFrom)
    {
        Detach((detatchFrom as AttachType));
    }
}

//A specific ability which doubles the dmg of a projectile created by a player
public class DblDmg : Ability<Player, Projectile>
{
    public DblDmg(int rank) : base(rank) { }
    public override void Apply(Player sender, Projectile applyTo)
    {
        //sender.UpgradedDmgRoundsFired += 1;
        //applyTo.dmg *= (int)(Math.Pow((double)2, (double)Rank));
    }
    public override void Attach(Player attachTo)
    {
        //attachTo.FireWeapon += Apply;
    }
    public override void Detach(Player detachFrom)
    {
        //detachFrom.FireWeapon -= Apply;
    }
}

//A specific ability which doubles the spd of a projectile created by a player
public class DblSpd : Ability<Player, Projectile>
{
    public DblSpd(int rank) : base(rank) { }
    public override void Apply(Player sender, Projectile applyTo)
    {
        //sender.UpgradedSpdRoundsFired += 1;
        //applyTo.speed *= (2 * Rank);
    }
    public override void Attach(Player attachTo)
    {
        //attachTo.FireWeapon += Apply;
    }
    public override void Detach(Player detachFrom)
    {
        //detachFrom.FireWeapon -= Apply;
    }
}
*/