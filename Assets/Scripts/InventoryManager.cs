using System;
using System.Collections;
using System.Collections.Generic;
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

    public Sprite GoldIcon;
    public List<LootItem> itemTypes = new List<LootItem>();
    public List<BagLoots> loots = new List<BagLoots>();
    public int goldAmount;

    private void Awake()
    {
        if(instance != null)
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

    public void AddLoot(int typeID,int amount)
    {
        bool found = false;
        foreach (BagLoots loot in loots)
        {
            if (loot.typeID == typeID)
            {
                loot.amount += amount;
                if(loot.amount <=0)
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
        foreach(BagLoots loot in loots)
        {
            if(loot.typeID == typeID)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
