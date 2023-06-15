using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem
{
    public int id;
    public int price;
}

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel, buySlotPrefab, sellSlotPrefab, shopSlotsHolder, buyButton, sellButton;
    private List<ShopItem> buyList, sellList;

    // Start is called before the first frame update
    void Start()
    {
        RefreshBuySlots();
        GenerateSellList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshBuySlots()
    {
        //Create Buy List
        GenerateBuyList();
        Debug.Log("Buy list count: " + buyList.Count);
        //Update UI
        UpdateSlotUI(buyList, 1);
        
    }

    public void RefreshSellSlots()
    {
        //Generate Sell list
        GenerateSellList();
        //Update UI
        UpdateSlotUI(sellList, 2);
    }

    public void UpdateSlotUI(List<ShopItem> slotsList, int type)
    {
        //Clear all slots
        if(shopSlotsHolder.transform.childCount > 0)
        {
            foreach (Transform child in shopSlotsHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        //Create new slots, type: 1 - buy; 2 - sell
        if(type == 1)
        {
            Debug.Log("update buy slots");
            foreach (ShopItem sitem in slotsList)
            {
                GameObject newSlot = Instantiate(buySlotPrefab, shopSlotsHolder.transform);
                newSlot.GetComponent<BuySlot>().UpdateSlotData(sitem.id, sitem.price);
                Debug.Log("update slot " + slotsList.IndexOf(sitem));
            }
        }
        else
        {
            foreach (ShopItem sitem in slotsList)
            {
                GameObject newSlot = Instantiate(sellSlotPrefab, shopSlotsHolder.transform);
                newSlot.GetComponent<SellSlot>().UpdateSlotData(sitem.id, sitem.price);
            }
        }
        
    }

    public void GenerateBuyList()
    {
        buyList = new List<ShopItem>();
        foreach (LootItem item in InventoryManager.instance.itemTypes)
        {
            ShopItem shopItem = new ShopItem();
            shopItem.id = item.ID;
            float randomFactor = Random.Range(0.9f, 1.2f);
            shopItem.price = Mathf.RoundToInt(randomFactor * item.baseValue);
            buyList.Add(shopItem);
        }
    }
    public void GenerateSellList()
    {
        sellList = new List<ShopItem>();
        foreach (BagLoots loot in InventoryManager.instance.loots)
        {
            ShopItem shopItem = new ShopItem();
            shopItem.id = loot.typeID;
            LootItem itemInfo = InventoryManager.instance.GetLootItemInfo(loot.typeID);
            float randomFactor = Random.Range(0.6f, 0.8f);
            shopItem.price = Mathf.RoundToInt(randomFactor * itemInfo.baseValue);
            sellList.Add(shopItem);
        }
    }

    public void UpdateSell()
    {
        foreach (BagLoots loot in InventoryManager.instance.loots)
        {
            bool found = false;
            foreach(ShopItem sell in sellList)
            {
                if (loot.typeID == sell.id)
                {
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                ShopItem shopItem = new ShopItem();
                shopItem.id = loot.typeID;
                LootItem itemInfo = InventoryManager.instance.GetLootItemInfo(loot.typeID);
                float randomFactor = Random.Range(0.6f, 0.8f);
                shopItem.price = Mathf.RoundToInt(randomFactor * itemInfo.baseValue);
                sellList.Add(shopItem);
            }
        }
    }

    public void BuyButton()
    {
        UpdateSlotUI(buyList,1);
        sellButton.GetComponent<Button>().interactable = true;
        buyButton.GetComponent<Button>().interactable = false;
    }
    public void SellButton()
    {
        UpdateSell();
        UpdateSlotUI(sellList,2);
        buyButton.GetComponent<Button>().interactable = true;
        sellButton.GetComponent<Button>().interactable = false;
    }
}
