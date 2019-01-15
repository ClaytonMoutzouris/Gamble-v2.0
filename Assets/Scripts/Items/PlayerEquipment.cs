using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    //Do it with dictionary, or with slots
    //We will start with slots so we can see it in inspector
    Armor head;
    Armor body;
    Armor gloves;
    Armor boots;
    Weapon leftHand;
    Weapon rightHand;

    public bool EquipArmor(Armor item)
    {
        switch (item.mSlot)
        {
            case EquipmentSlot.Head:
                head = item;
                return true;
            case EquipmentSlot.Body:
                body = item;
                return true;
            case EquipmentSlot.Gloves:
                gloves = item;
                return true;
            case EquipmentSlot.Boots:
                boots = item;
                return true;
        }

        return false;
    }

    public bool EquipWeapon(Weapon item)
    {
        switch (item.mSlot)
        {
            case EquipmentSlot.LeftHand:
                leftHand = item;
                return true;
            case EquipmentSlot.RightHand:
                rightHand = item;
                return true;

        }

        return false;
    }

}
