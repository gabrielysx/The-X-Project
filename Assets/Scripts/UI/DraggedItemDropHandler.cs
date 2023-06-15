using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EquipmentType
{
    Equipment, Weapon
}

public class DraggedItemDropHandler : MonoBehaviour, IDropHandler
{

    [SerializeField] private EquipmentType type;

    public void OnDrop(PointerEventData eventData)
    {
        DragableItem dr = eventData.pointerDrag.GetComponent<DragableItem>();
        switch (type)
        {
            case EquipmentType.Weapon:
                WeaponSlot weaponSlot = GetComponent<WeaponSlot>();
                //only process when the type matches
                if (dr.curType == EquipmentType.Weapon)
                {
                    InventoryManager.instance.RemoveEquipment(dr.GetParentInventorySlotIndex());
                    if(weaponSlot.slotID == 0)
                    {
                        InventoryManager.instance.UpdateLeftWeaponSlot(dr.GetCurrentItemID());
                    }
                    else
                    {
                        InventoryManager.instance.UpdateRightWeaponSlot(dr.GetCurrentItemID());
                    }
                    UIManager.instance.UpdateInventorySlots();
                }
                break;
            case EquipmentType.Equipment:
                
                EquipmentSlot equipSlot = GetComponent<EquipmentSlot>();
                //only process when the type matches
                if(dr.curType == EquipmentType.Equipment)
                {
                    InventoryManager.instance.RemoveEquipment(dr.GetParentInventorySlotIndex());
                    InventoryManager.instance.UpdateEquipmentSlot(equipSlot.slotID, dr.GetCurrentItemID());
                    UIManager.instance.UpdateInventorySlots();
                }
                break;
            default:
                break;
        }
    }
}