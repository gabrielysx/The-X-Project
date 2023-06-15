using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BagLoots
{
    public int typeID;
    public int amount;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private PlayerAttackManager attackManagerInstance;

    public Sprite GoldIcon;
    public List<LootItem> itemTypes = new List<LootItem>();
    public List<EquipmentItem> equipmentTypes = new List<EquipmentItem>();
    public List<BagLoots> loots = new List<BagLoots>();
    public List<int> equipments = new List<int>();
    public int leftWeapon, rightWeapon;
    public List<int> equipmentSlots = new List<int> { -1, -1, -1, -1 };
    public int goldAmount;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        attackManagerInstance = Character.mainPlayerInstance.gameObject.GetComponent<PlayerAttackManager>();
    }

    public void AddLoot(int typeID, int amount)
    {
        bool found = false;
        foreach (BagLoots loot in loots)
        {
            if (loot.typeID == typeID)
            {
                loot.amount += amount;
                if (loot.amount <= 0)
                {
                    loots.Remove(loot);
                }
                found = true;
                break;
            }
        }
        if (!found && amount > 0)
        {
            BagLoots newLoot = new BagLoots();
            newLoot.typeID = typeID;
            newLoot.amount = amount;
            loots.Add(newLoot);
        }
    }

    public int CheckLootAmount(int typeID)
    {
        foreach (BagLoots loot in loots)
        {
            if (loot.typeID == typeID)
            {
                return loot.amount;
            }
        }
        //Not found then return 0
        return 0;
    }

    public LootItem GetLootItemInfo(int typeID)
    {
        return itemTypes[typeID];
    }

    public void AddGold(int amount)
    {
        goldAmount += amount;
    }

    public void AddEquipment(int typeID)
    {
        equipments.Add(typeID);
    }

    public void RemoveEquipment(int index)
    {
        equipments.RemoveAt(index);
    }

    public void UpdateEquipmentSlot(int index, int newID)
    {
        if (equipmentSlots[index] != -1)
        {
            //equipment slot is full
            //Return the equipment to the inventory
            int oldID = equipmentSlots[index];
            equipments.Add(oldID);
        }
        //Set the new equipment to the slot
        equipmentSlots[index] = newID;
    }

    public void UpdateLeftWeaponSlot(int newID)
    {
        if (leftWeapon != -1)
        {
            //Left weapon slot is full
            //Return the weapon to the inventory
            int oldID = leftWeapon;
            equipments.Add(oldID);
        }
        //Set the new equipment to the slot
        leftWeapon = newID;
        //Update the player attack manager
        Character.mainPlayerInstance.GetComponent<PlayerAttackManager>().LeftSlotUpdate(leftWeapon);
    }
    public void UpdateRightWeaponSlot(int newID)
    {
        if (rightWeapon != -1)
        {
            //Right weapon slot is full
            //Return the weapon to the inventory
            int oldID = rightWeapon;
            equipments.Add(oldID);
        }
        //Set the new equipment to the slot
        rightWeapon = newID;
        //Update the player attack manager
        Character.mainPlayerInstance.GetComponent<PlayerAttackManager>().RightSlotUpdate(rightWeapon);
    }

}
