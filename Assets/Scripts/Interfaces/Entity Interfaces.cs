using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Jump is going to be my first try using interfaces
public interface IJump
{
    float JumpSpeed { get; set; }
    bool DoubleJump { get; set; }
    void Jump();
}

public interface IHurtable
{
    Hurtbox HurtBox { get; set; }
    void GetHurt(Attack attack);

}