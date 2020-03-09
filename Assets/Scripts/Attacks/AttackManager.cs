using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Right now, only players have an attackmanager
//It might be better if attack manager was abolished and moved into each class that can make attacks
//This class really only has a list of attacks and updates them
[SerializeField]
public class AttackManager {

    public Entity mEntity;

    public List<RangedAttack> rangedAttacks;
    public List<MeleeAttack> meleeAttacks;

    public AttackManager(Entity entity)
    {
        mEntity = entity;
        rangedAttacks = new List<RangedAttack>();

        meleeAttacks = new List<MeleeAttack>();
    }

    public void UpdateAttacks()
    {
        foreach(RangedAttack attack in rangedAttacks)
        {
                attack.UpdateAttack();  
        }

        foreach (MeleeAttack attack in meleeAttacks)
        {
            attack.UpdateAttack();
        }

    }

    public void SecondUpdate()
    {

        foreach (RangedAttack attack in rangedAttacks)
        {
            if (attack.mIsActive)
                attack.SecondUpdate();
        }

        foreach (MeleeAttack attack in meleeAttacks)
        {
            if (attack.mIsActive)
                attack.SecondUpdate();
        }


    }


    public bool IsAttacking()
    {
        foreach (RangedAttack attack in rangedAttacks)
        {
            if (attack.mIsActive)
            {
                return true;
            }
        }

        foreach (MeleeAttack attack in meleeAttacks)
        {
            if (attack.mIsActive)
            {
                return true;
            }
        }
        return false;
    }

}
