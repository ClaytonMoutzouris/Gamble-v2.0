using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Right now, only players have an attackmanager
//It might be better if attack manager was abolished and moved into each class that can make attacks
//This class really only has a list of attacks and updates them
[SerializeField]
public class AttackManager : MonoBehaviour {

    public Entity mEntity;
    public List<Attack> AttackList = new List<Attack>();
    public List<MeleeAttack> meleeAttacks;

    private void Start()
    {
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


}
