using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbeltUI : MonoBehaviour
{
    public MeleeWeaponToolbeltNode meleeNode;
    public RangedWeaponToolbeltNode rangedNode;
    public GadgetToolbeltNode gadgetNode1;
    public GadgetToolbeltNode gadgetNode2;
    public QuickUseToolbeltNode quickNode;

    public PlayerPanel parentPanel;

    public void Update()
    {

    }

    public void UpdateEquipmentNode(EquipmentSlot equipmentSlot)
    {

        Equipment equipment = equipmentSlot.GetContents();

        switch (equipmentSlot.GetSlotType())
        {
            case EquipmentSlotType.Offhand:
                if(equipment != null)
                {
                    meleeNode.nodeIcon.sprite = equipment.sprite;
                } else
                {
                    meleeNode.nodeIcon.sprite = null;
                }
                break;
            case EquipmentSlotType.Mainhand:
                if (equipment != null)
                {
                    rangedNode.nodeIcon.sprite = equipment.sprite;
                }
                else
                {
                    rangedNode.nodeIcon.sprite = null;
                }
                break;
            case EquipmentSlotType.Gadget:
                //Haven't figured out gadgets yet (due to multiple slots)
                break;
            default:
                //Do nothing
                break;
        }
    }

    public void UpdateQuickUseNode(ConsumableItem item)
    {
        if(item != null)
        {
            quickNode.nodeIcon.sprite = item.sprite;
        } else
        {
            quickNode.nodeIcon.sprite = null;
        }
    }
}
