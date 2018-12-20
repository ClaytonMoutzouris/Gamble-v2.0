using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class AttackManager : MonoBehaviour {

    public Entity mEntity;
    public List<Attack> AttackList = new List<Attack>();

    public List<MeleeAttack> meleeAttacks;
    public List<RangedAttack> rangedAttacks;


    private void Start()
    {
        if (AttackList == null)
        {
            AttackList = new List<Attack>();
        }


        if (meleeAttacks == null)
        {
            meleeAttacks = new List<MeleeAttack>();
        }

        if (rangedAttacks == null)
        {
            rangedAttacks = new List<RangedAttack>();
        }

        foreach (MeleeAttack meleeAttack in meleeAttacks)
        {
            AttackList.Add(meleeAttack);

        }

        foreach (RangedAttack rangedAttack in rangedAttacks)
        {
            AttackList.Add(rangedAttack);

        }


        mEntity = GetComponent<Entity>();
    }

    public void UpdateAttacks()
    {
        foreach(Attack attack in AttackList)
        {
                attack.UpdateAttack();  
        }


    }

    public void SecondUpdate()
    {

        //this makes sure the attack hitbox follows the player
        foreach (Attack attack in AttackList)
        {
            if (attack.mIsActive)
                attack.SecondUpdate();
        }

           
    }

    void OnDrawGizmos()
    {
        foreach (Attack attack in AttackList)
        {
            if (attack.mIsActive)
            {
                if(attack is MeleeAttack)
                {
                    MeleeAttack temp = (MeleeAttack)attack;

                    //calculate the position of the aabb's center
                    var aabbPos = temp.hitbox.mAABB.Center;
                    var halfSize = temp.hitbox.mAABB.HalfSize;
                    //Debug.Log(aabbPos + " halfsize =" + temp.hitbox.collider.HalfSize);

                    //draw the aabb rectangle
                    Gizmos.color = new Color(0, 1, 0, 1);
                    Gizmos.DrawCube(aabbPos, halfSize * 2.0f);
                }
                
            }
        }
    }

    protected void DrawAttackGizmos()
    {
        
    }

}
